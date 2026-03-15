import { McpServer } from "@modelcontextprotocol/sdk/server/mcp.js";
import { StdioServerTransport } from "@modelcontextprotocol/sdk/server/stdio.js";
import { z } from "zod";
import { WebSocketServer, WebSocket } from "ws";
import { v4 as uuidv4 } from "uuid";

// ---------------------------------------------------------------------------
// Logging (stderr only — stdout is reserved for MCP protocol)
// ---------------------------------------------------------------------------
function log(msg: string) {
  process.stderr.write(`[sts2-mcp] ${msg}\n`);
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
  data: CombatState;
}

interface CommandResponseMessage {
  type: "command_response";
  id: string;
  success: boolean;
  error: string | null;
  data?: unknown;
}

type ModMessage = StateUpdateMessage | CommandResponseMessage;

interface CombatState {
  turn: number;
  player: {
    hp: number;
    max_hp: number;
    block: number;
    energy: number;
    max_energy: number;
    gold: number;
    powers: Array<{ name: string; amount: number; type: string }>;
    hand: Array<{
      index: number;
      name: string;
      type: string;
      energy_cost: number;
      target_type: string;
      can_play: boolean;
      description: string;
    }>;
    draw_pile_count: number;
    discard_pile_count: number;
    exhaust_pile_count: number;
    relics: string[];
    potions: Array<{ slot: number; name: string | null; target_type: string | null }>;
  };
  enemies: Array<{
    index: number;
    name: string;
    hp: number;
    max_hp: number;
    block: number;
    powers: Array<{ name: string; amount: number; type: string }>;
    intents: Array<{ type: string; damage: number; hits: number }>;
  }>;
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
const COMMAND_TIMEOUT_MS = 10_000;

let latestCombatState: CombatState | null = null;
let modSocket: WebSocket | null = null;
const pendingRequests = new Map<string, PendingRequest>();

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
        latestCombatState = msg.data;
        log(`State update received (turn ${msg.data.turn}, ${msg.data.enemies.length} enemies)`);
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
// MCP Server
// ---------------------------------------------------------------------------
const server = new McpServer({
  name: "sts2",
  version: "0.1.0",
});

// --- get_combat_state ---
server.tool(
  "get_combat_state",
  "Get the current STS2 combat state including player HP/energy/hand, enemies, and intents. Returns cached state pushed by the game mod at each turn start.",
  {},
  async () => {
    if (!latestCombatState) {
      return {
        content: [
          {
            type: "text" as const,
            text: modSocket
              ? "No combat state available yet. Enter a combat in STS2 to receive state updates."
              : "Game mod is not connected. Start STS2 with the mod loaded.",
          },
        ],
      };
    }

    return {
      content: [
        {
          type: "text" as const,
          text: JSON.stringify(latestCombatState, null, 2),
        },
      ],
    };
  }
);

// --- play_card ---
server.tool(
  "play_card",
  "Play a card from hand by index. For cards targeting a single enemy (target_type=AnyEnemy), provide target_index. For non-targeted cards, omit target_index.",
  {
    hand_index: z
      .number()
      .int()
      .min(0)
      .describe("Index of the card in hand (0-based, from get_combat_state)"),
    target_index: z
      .number()
      .int()
      .min(0)
      .optional()
      .describe(
        "Index of the target enemy (0-based, from get_combat_state enemies array). Required for AnyEnemy cards."
      ),
  },
  async ({ hand_index, target_index }) => {
    try {
      const resp = await sendCommand("play_card", {
        hand_index,
        target_index: target_index ?? -1,
      });

      if (!resp.success) {
        return {
          content: [{ type: "text" as const, text: `Failed to play card: ${resp.error}` }],
        };
      }

      return {
        content: [{ type: "text" as const, text: `Card at index ${hand_index} played successfully.` }],
      };
    } catch (e) {
      return {
        content: [
          {
            type: "text" as const,
            text: `Error: ${e instanceof Error ? e.message : String(e)}`,
          },
        ],
      };
    }
  }
);

// --- end_turn ---
server.tool(
  "end_turn",
  "End the current player turn. Call this after playing all desired cards.",
  {},
  async () => {
    try {
      const resp = await sendCommand("end_turn");

      if (!resp.success) {
        return {
          content: [{ type: "text" as const, text: `Failed to end turn: ${resp.error}` }],
        };
      }

      return {
        content: [{ type: "text" as const, text: "Turn ended." }],
      };
    } catch (e) {
      return {
        content: [
          {
            type: "text" as const,
            text: `Error: ${e instanceof Error ? e.message : String(e)}`,
          },
        ],
      };
    }
  }
);

// --- use_potion ---
server.tool(
  "use_potion",
  "Use a potion from the potion belt by slot index. For targeted potions, provide target_index.",
  {
    slot_index: z
      .number()
      .int()
      .min(0)
      .describe("Potion slot index (0-based, from get_combat_state potions array)"),
    target_index: z
      .number()
      .int()
      .min(0)
      .optional()
      .describe(
        "Index of the target enemy (0-based). Required for targeted potions."
      ),
  },
  async ({ slot_index, target_index }) => {
    try {
      const resp = await sendCommand("use_potion", {
        slot_index,
        target_index: target_index ?? -1,
      });

      if (!resp.success) {
        return {
          content: [{ type: "text" as const, text: `Failed to use potion: ${resp.error}` }],
        };
      }

      return {
        content: [
          { type: "text" as const, text: `Potion at slot ${slot_index} used successfully.` },
        ],
      };
    } catch (e) {
      return {
        content: [
          {
            type: "text" as const,
            text: `Error: ${e instanceof Error ? e.message : String(e)}`,
          },
        ],
      };
    }
  }
);

// ---------------------------------------------------------------------------
// Start
// ---------------------------------------------------------------------------
async function main() {
  log("Starting STS2 MCP server...");
  const transport = new StdioServerTransport();
  await server.connect(transport);
  log("MCP server connected via stdio");
}

main().catch((err) => {
  log(`Fatal error: ${err instanceof Error ? err.message : String(err)}`);
  process.exit(1);
});
