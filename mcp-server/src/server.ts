import { createServer, IncomingMessage, ServerResponse } from "http";
import { WebSocketServer, WebSocket } from "ws";
import { v4 as uuidv4 } from "uuid";
import { exec } from "child_process";

// ---------------------------------------------------------------------------
// Logging
// ---------------------------------------------------------------------------
function log(msg: string) {
  process.stderr.write(`[sts2-server] ${msg}\n`);
}

// ---------------------------------------------------------------------------
// Types
// ---------------------------------------------------------------------------
interface CommandResponse {
  id: string;
  success: boolean;
  error: string | null;
  data?: unknown;
}

interface StateUpdateMessage {
  type: "state_update";
  screen?: string;
  settled?: boolean;
  data: Record<string, unknown>;
}

interface CommandResponseMessage {
  type: "command_response";
  id: string;
  success: boolean;
  error: string | null;
  data?: unknown;
}

interface PendingChoiceData {
  choice_type: string;
  prompt: string;
  min_selections: number;
  max_selections: number;
  cards: CardInfo[];
}

interface PendingChoiceMessage {
  type: "pending_choice";
  data: PendingChoiceData;
}

type ModMessage = StateUpdateMessage | CommandResponseMessage | PendingChoiceMessage;

interface PowerInfo {
  id: string;
  name: string;
  power_type: string;
  amount: number;
  description: string | null;
}

interface CardInfo {
  index: number;
  id: string;
  name: string;
  card_type: string;
  rarity: string;
  energy_cost: number;
  target_type: string;
  can_play: boolean;
  is_upgraded: boolean;
  description: string;
  keywords?: string[];
}

interface RelicInfo {
  id: string;
  name: string;
  counter?: number;
  description?: string;
  rarity?: string;
}

interface PotionInfo {
  slot: number;
  name: string | null;
  target_type: string | null;
  description: string | null;
}

interface EnemyInfo {
  index: number;
  id: string | null;
  name: string;
  hp: number;
  max_hp: number;
  block: number;
  powers: PowerInfo[];
  intents: Array<{ type: string; damage: number; hits: number; description?: string }>;
}

interface CombatState {
  turn: number;
  player: {
    hp: number;
    max_hp: number;
    block: number;
    energy: number;
    max_energy: number;
    gold: number;
    powers: PowerInfo[];
    hand: CardInfo[];
    draw_pile_count: number;
    discard_pile_count: number;
    exhaust_pile_count: number;
    relics: RelicInfo[];
    potions: PotionInfo[];
  };
  enemies: EnemyInfo[];
}

interface PendingRequest {
  resolve: (value: CommandResponse) => void;
  reject: (reason: Error) => void;
  timeout: ReturnType<typeof setTimeout>;
}

// ---------------------------------------------------------------------------
// State
// ---------------------------------------------------------------------------
const WS_PORT = 19280;
const HTTP_PORT = parseInt(process.env.STS2_HTTP_PORT || "19281");
const COMMAND_TIMEOUT_MS = 10_000;

const STS2_APP_PATH =
  process.env.STS2_INSTALL_DIR ||
  "/Users/simgeon-u/Library/Application Support/Steam/steamapps/common/Slay the Spire 2/SlayTheSpire2.app";

// Combat state (turn-based, pushed by AutoPlayPatch)
let latestCombatState: CombatState | null = null;

// General game state (non-combat screens: map, shop, rest, event, rewards)
let latestGameState: Record<string, unknown> | null = null;
let currentScreen: string | null = null;

let modSocket: WebSocket | null = null;
const pendingRequests = new Map<string, PendingRequest>();
let settledStateResolve: ((state: Record<string, unknown>) => void) | null = null;

// Pending card selection state
let pendingChoice: PendingChoiceData | null = null;
let pendingChoiceResolve: ((choice: PendingChoiceData) => void) | null = null;

// Relic detail cache: stores full descriptions from first encounter
const relicCache = new Map<string, RelicInfo>();

/**
 * Merge relic details: cache new descriptions, fill in cached descriptions
 * for subsequent updates that omit them.
 */
function mergeRelicCache(relics: RelicInfo[]): RelicInfo[] {
  for (const relic of relics) {
    if (relic.description) {
      // New or updated relic info — cache it
      relicCache.set(relic.id, { ...relic });
    } else {
      // No description in this update — fill from cache
      const cached = relicCache.get(relic.id);
      if (cached) {
        relic.description = cached.description;
        relic.rarity = relic.rarity ?? cached.rarity;
      }
    }
    // Always update counter from latest state
    const cached = relicCache.get(relic.id);
    if (cached && relic.counter !== undefined) {
      cached.counter = relic.counter;
    }
  }
  return relics;
}

// ---------------------------------------------------------------------------
// Wait for a settled state_update from the game mod (pushed after effects resolve)
// ---------------------------------------------------------------------------
function waitForSettledState(timeoutMs: number): Promise<Record<string, unknown> | null> {
  return new Promise((resolve) => {
    const timer = setTimeout(() => {
      settledStateResolve = null;
      log(`Settled state wait timed out after ${timeoutMs}ms, using latest state`);
      resolve(null);
    }, timeoutMs);

    settledStateResolve = (state: Record<string, unknown>) => {
      clearTimeout(timer);
      settledStateResolve = null;
      resolve(state);
    };
  });
}

// ---------------------------------------------------------------------------
// Wait for a pending_choice message from the game mod (card selection prompt)
// ---------------------------------------------------------------------------
function waitForPendingChoice(timeoutMs: number): Promise<PendingChoiceData | null> {
  // If there's already a pending choice, return it immediately
  if (pendingChoice) {
    return Promise.resolve(pendingChoice);
  }
  return new Promise((resolve) => {
    const timer = setTimeout(() => {
      pendingChoiceResolve = null;
      resolve(null);
    }, timeoutMs);

    pendingChoiceResolve = (choice: PendingChoiceData) => {
      clearTimeout(timer);
      pendingChoiceResolve = null;
      resolve(choice);
    };
  });
}

// ---------------------------------------------------------------------------
// WebSocket Server (game mod connects here)
// ---------------------------------------------------------------------------
const wss = new WebSocketServer({ port: WS_PORT });

wss.on("listening", () => {
  log(`WebSocket server listening on ws://localhost:${WS_PORT}`);
});

wss.on("connection", (ws) => {
  log("Game mod connected");

  // Only allow one mod connection at a time
  if (modSocket && modSocket.readyState === WebSocket.OPEN) {
    log("Replacing existing mod connection");
    modSocket.close();
  }
  modSocket = ws;

  ws.on("message", (raw) => {
    try {
      const msg = JSON.parse(raw.toString()) as ModMessage;

      if (msg.type === "state_update") {
        const screen = msg.screen ?? null;
        const settled = msg.settled ?? false;
        const data = msg.data;

        if (screen && screen !== "combat") {
          // Non-combat state update (map, shop, rest, event, rewards)
          currentScreen = screen;
          latestGameState = data;
          log(`Game state update: screen=${screen}, settled=${settled}`);
        } else {
          // Combat state update (existing behavior)
          if (data.player && (data.player as Record<string, unknown>).relics) {
            (data.player as Record<string, unknown>).relics = mergeRelicCache(
              (data.player as Record<string, unknown>).relics as RelicInfo[]
            );
          }
          latestCombatState = data as unknown as CombatState;
          currentScreen = "combat";
          log(`State update received (turn ${(data as Record<string, unknown>).turn}, ${((data as Record<string, unknown>).enemies as unknown[])?.length ?? 0} enemies, settled=${settled})`);
        }

        if (settled && settledStateResolve) {
          settledStateResolve(data);
        }
      } else if (msg.type === "pending_choice") {
        pendingChoice = msg.data;
        log(`Pending choice: ${msg.data.choice_type} (${msg.data.cards.length} cards, select ${msg.data.min_selections}-${msg.data.max_selections})`);
        if (pendingChoiceResolve) {
          pendingChoiceResolve(msg.data);
        }
      } else if (msg.type === "command_response") {
        const pending = pendingRequests.get(msg.id);
        if (pending) {
          clearTimeout(pending.timeout);
          pendingRequests.delete(msg.id);
          pending.resolve({
            id: msg.id,
            success: msg.success,
            error: msg.error,
            data: msg.data,
          });
        } else {
          log(`Received response for unknown request: ${msg.id}`);
        }
      }
    } catch (e) {
      log(`Failed to parse message: ${e}`);
    }
  });

  ws.on("close", () => {
    log("Game mod disconnected");
    if (modSocket === ws) {
      modSocket = null;
      relicCache.clear();
      pendingChoice = null;
      latestGameState = null;
      currentScreen = null;
    }
    // Reject all pending requests
    for (const [id, req] of pendingRequests.entries()) {
      clearTimeout(req.timeout);
      req.reject(new Error("Mod disconnected"));
      pendingRequests.delete(id);
    }
  });

  ws.on("error", (err) => {
    log(`WebSocket error: ${err.message}`);
  });
});

// ---------------------------------------------------------------------------
// Send command to mod and await response
// ---------------------------------------------------------------------------
function sendCommand(
  command: string,
  params: Record<string, unknown> = {}
): Promise<CommandResponse> {
  return new Promise((resolve, reject) => {
    if (!modSocket || modSocket.readyState !== WebSocket.OPEN) {
      reject(new Error("Game mod is not connected. Start STS2 with the mod loaded."));
      return;
    }

    const id = uuidv4();

    const timeout = setTimeout(() => {
      if (pendingRequests.has(id)) {
        pendingRequests.delete(id);
        reject(new Error(`Command '${command}' timed out after ${COMMAND_TIMEOUT_MS}ms`));
      }
    }, COMMAND_TIMEOUT_MS);

    pendingRequests.set(id, { resolve, reject, timeout });

    const message = JSON.stringify({ id, command, params });
    modSocket.send(message);
    log(`Sent command: ${command} (id=${id})`);
  });
}

// ---------------------------------------------------------------------------
// HTTP helpers
// ---------------------------------------------------------------------------
function readBody(req: IncomingMessage): Promise<Record<string, unknown>> {
  return new Promise((resolve) => {
    let body = "";
    req.on("data", (chunk: Buffer) => (body += chunk));
    req.on("end", () => {
      try {
        resolve(JSON.parse(body));
      } catch {
        resolve({});
      }
    });
  });
}

function json(res: ServerResponse, data: unknown, status = 200) {
  res.writeHead(status, { "Content-Type": "application/json" });
  res.end(JSON.stringify(data, null, 2));
}

function errJson(res: ServerResponse, message: string, status = 500) {
  json(res, { error: message }, status);
}

// ---------------------------------------------------------------------------
// Game launch/quit helpers
// ---------------------------------------------------------------------------
function launchGame(): Promise<string> {
  return new Promise((resolve, reject) => {
    exec(`open "${STS2_APP_PATH}"`, (err) => {
      if (err) reject(new Error(`Failed to launch game: ${err.message}`));
      else resolve("Game launch command sent");
    });
  });
}

function quitGame(): Promise<string> {
  return new Promise((resolve, reject) => {
    exec(`osascript -e 'quit app "SlayTheSpire2"'`, (err) => {
      if (err) reject(new Error(`Failed to quit game: ${err.message}`));
      else resolve("Game quit command sent");
    });
  });
}

// ---------------------------------------------------------------------------
// HTTP Route Handlers
// ---------------------------------------------------------------------------

async function handleStatus(_req: IncomingMessage, res: ServerResponse) {
  json(res, {
    ok: true,
    game_connected: modSocket !== null && modSocket.readyState === WebSocket.OPEN,
    current_screen: currentScreen,
  });
}

async function handleGetState(_req: IncomingMessage, res: ServerResponse) {
  if (!modSocket) {
    json(res, {
      screen: "disconnected",
      message: "Game mod is not connected. Start STS2 with the mod loaded.",
    });
    return;
  }

  const response: Record<string, unknown> = {
    screen: currentScreen ?? "unknown",
  };

  if (currentScreen === "combat" && latestCombatState) {
    response.combat = latestCombatState;
    if (pendingChoice) {
      response.pending_choice = pendingChoice;
    }
  } else if (latestGameState) {
    response.data = latestGameState;
  } else {
    response.message = "No game state available yet. Start a run in STS2.";
  }

  json(res, response);
}

async function handleGetCombatState(_req: IncomingMessage, res: ServerResponse) {
  if (!latestCombatState) {
    json(res, {
      error: modSocket
        ? "No combat state available yet. Enter a combat in STS2 to receive state updates."
        : "Game mod is not connected. Start STS2 with the mod loaded.",
    });
    return;
  }

  const response: Record<string, unknown> = { ...latestCombatState };
  if (pendingChoice) {
    response.pending_choice = pendingChoice;
  }
  json(res, response);
}

async function handlePlayCard(req: IncomingMessage, res: ServerResponse) {
  const body = await readBody(req);
  const hand_index = body.hand_index as number;
  const target_index = (body.target_index as number) ?? -1;

  try {
    const resp = await sendCommand("play_card", { hand_index, target_index });
    if (!resp.success) {
      json(res, { error: `Failed to play card: ${resp.error}` }, 400);
      return;
    }

    const settledState = await waitForSettledState(5000);
    const updatedState = settledState ?? latestCombatState;
    json(res, { result: "Card played successfully", updated_state: updatedState });
  } catch (e) {
    errJson(res, e instanceof Error ? e.message : String(e));
  }
}

async function handleEndTurn(_req: IncomingMessage, res: ServerResponse) {
  try {
    const resp = await sendCommand("end_turn");
    if (!resp.success) {
      json(res, { error: `Failed to end turn: ${resp.error}` }, 400);
      return;
    }

    // Wait briefly for pending_choice (e.g., Stratagem triggers after shuffle)
    const choice = await waitForPendingChoice(3000);
    if (choice) {
      json(res, { result: "Turn ended. Card selection required.", pending_choice: choice });
      return;
    }

    json(res, { result: "Turn ended." });
  } catch (e) {
    errJson(res, e instanceof Error ? e.message : String(e));
  }
}

async function handleUsePotion(req: IncomingMessage, res: ServerResponse) {
  const body = await readBody(req);
  const slot_index = body.slot_index as number;
  const target_index = (body.target_index as number) ?? -1;

  try {
    const resp = await sendCommand("use_potion", { slot_index, target_index });
    if (!resp.success) {
      json(res, { error: `Failed to use potion: ${resp.error}` }, 400);
      return;
    }

    const settledState = await waitForSettledState(5000);
    const updatedState = settledState ?? latestCombatState;
    json(res, { result: "Potion used successfully", updated_state: updatedState });
  } catch (e) {
    errJson(res, e instanceof Error ? e.message : String(e));
  }
}

async function handleSelectCards(req: IncomingMessage, res: ServerResponse) {
  const body = await readBody(req);
  const card_indices = body.card_indices as number[];

  if (!pendingChoice) {
    json(res, { error: "No pending card selection. Use GET /state to check for pending_choice." }, 400);
    return;
  }

  try {
    const resp = await sendCommand("select_cards", { card_indices });
    if (!resp.success) {
      json(res, { error: `Failed to select cards: ${resp.error}` }, 400);
      return;
    }

    pendingChoice = null;

    const settledState = await waitForSettledState(5000);
    const updatedState = settledState ?? latestCombatState;
    json(res, { result: "Cards selected successfully", updated_state: updatedState });
  } catch (e) {
    errJson(res, e instanceof Error ? e.message : String(e));
  }
}

async function handleChooseAction(req: IncomingMessage, res: ServerResponse) {
  const body = await readBody(req);
  const params: Record<string, unknown> = { action_type: body.action_type };
  if (body.node_index !== undefined) params.node_index = body.node_index;
  if (body.reward_index !== undefined) params.reward_index = body.reward_index;
  if (body.card_index !== undefined) params.card_index = body.card_index;
  if (body.option_index !== undefined) params.option_index = body.option_index;
  if (body.item_index !== undefined) params.item_index = body.item_index;

  try {
    const resp = await sendCommand("choose_action", params);
    if (!resp.success) {
      json(res, { error: `Failed: ${resp.error}` }, 400);
      return;
    }

    // Wait for settled state (room transition may push new state)
    const settledState = await waitForSettledState(5000);

    const result: Record<string, unknown> = {
      result: `Action '${body.action_type}' executed successfully`,
    };

    if (settledState) {
      result.updated_state = settledState;
    } else if (latestGameState) {
      result.current_state = { screen: currentScreen, data: latestGameState };
    }

    json(res, result);
  } catch (e) {
    errJson(res, e instanceof Error ? e.message : String(e));
  }
}

async function handleLaunchGame(_req: IncomingMessage, res: ServerResponse) {
  // Check if already connected
  if (modSocket && modSocket.readyState === WebSocket.OPEN) {
    json(res, { result: "Game is already running and connected.", game_connected: true });
    return;
  }

  try {
    await launchGame();
    log("Game launch command sent, waiting for WebSocket connection...");

    // Wait for game mod to connect via WebSocket (up to 30s)
    const connected = await new Promise<boolean>((resolve) => {
      const checkInterval = setInterval(() => {
        if (modSocket && modSocket.readyState === WebSocket.OPEN) {
          clearInterval(checkInterval);
          clearTimeout(timeout);
          resolve(true);
        }
      }, 500);

      const timeout = setTimeout(() => {
        clearInterval(checkInterval);
        resolve(false);
      }, 30_000);
    });

    if (connected) {
      json(res, { result: "Game launched and mod connected.", game_connected: true });
    } else {
      json(res, {
        result: "Game launch command sent, but mod has not connected yet. It may still be loading.",
        game_connected: false,
      });
    }
  } catch (e) {
    errJson(res, e instanceof Error ? e.message : String(e));
  }
}

async function handleQuitGame(_req: IncomingMessage, res: ServerResponse) {
  try {
    await quitGame();
    log("Game quit command sent");
    json(res, { result: "Game quit command sent.", game_connected: false });
  } catch (e) {
    errJson(res, e instanceof Error ? e.message : String(e));
  }
}

async function handleTools(_req: IncomingMessage, res: ServerResponse) {
  json(res, {
    endpoints: [
      { method: "GET", path: "/status", description: "Server status and game connection state" },
      { method: "GET", path: "/state", description: "Current game state (combat/map/shop/rest/event/rewards)" },
      { method: "GET", path: "/combat_state", description: "Current combat state (player/enemies/hand)" },
      { method: "POST", path: "/play_card", description: "Play a card: {hand_index, target_index?}" },
      { method: "POST", path: "/end_turn", description: "End the current turn" },
      { method: "POST", path: "/use_potion", description: "Use a potion: {slot_index, target_index?}" },
      { method: "POST", path: "/select_cards", description: "Select cards from pending choice: {card_indices: number[]}" },
      { method: "POST", path: "/choose_action", description: "Non-combat action: {action_type, node_index?, reward_index?, card_index?, option_index?, item_index?}" },
      { method: "POST", path: "/launch_game", description: "Launch STS2 and wait for mod connection" },
      { method: "POST", path: "/quit_game", description: "Quit STS2" },
      { method: "GET", path: "/tools", description: "This endpoint list" },
    ],
  });
}

// ---------------------------------------------------------------------------
// HTTP Server
// ---------------------------------------------------------------------------
const httpServer = createServer(async (req, res) => {
  const url = req.url ?? "/";
  const method = req.method ?? "GET";

  try {
    if (url === "/status" && method === "GET") await handleStatus(req, res);
    else if ((url === "/state" || url === "/game_state") && method === "GET") await handleGetState(req, res);
    else if (url === "/combat_state" && method === "GET") await handleGetCombatState(req, res);
    else if (url === "/play_card" && method === "POST") await handlePlayCard(req, res);
    else if (url === "/end_turn" && method === "POST") await handleEndTurn(req, res);
    else if (url === "/use_potion" && method === "POST") await handleUsePotion(req, res);
    else if (url === "/select_cards" && method === "POST") await handleSelectCards(req, res);
    else if (url === "/choose_action" && method === "POST") await handleChooseAction(req, res);
    else if (url === "/launch_game" && method === "POST") await handleLaunchGame(req, res);
    else if (url === "/quit_game" && method === "POST") await handleQuitGame(req, res);
    else if (url === "/tools" && method === "GET") await handleTools(req, res);
    else errJson(res, "Not found", 404);
  } catch (e) {
    errJson(res, e instanceof Error ? e.message : String(e));
  }
});

// ---------------------------------------------------------------------------
// Start
// ---------------------------------------------------------------------------
httpServer.listen(HTTP_PORT, "127.0.0.1", () => {
  log(`STS2 server started — HTTP on http://localhost:${HTTP_PORT}, WebSocket on ws://localhost:${WS_PORT}`);
});
