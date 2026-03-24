---
name: sts2-play
description: Play STS2 via HTTP server. Use when user asks to "play STS2", "play combat", "STS2 전투", "자동 플레이", or when combat state is available.
---

# STS2 Auto-Play (OpenClaw)

Play Slay the Spire 2 in real-time via HTTP server.

## Setup

### 1. Start the server (one-time per session)

```bash
node /Users/simgeon-u/Documents/dev/side-project/sts2_ExampleMod/mcp-server/dist/server.js &
```

Run in background via `exec` with `background: true`. The server:
- Opens WebSocket on port 19280 for game mod connection
- Exposes HTTP API on port 19281

### 2. Check status

```bash
curl -s http://localhost:19281/status
```

### 3. Launch/quit game

```bash
# Launch STS2 (waits up to 30s for mod connection)
curl -s -X POST http://localhost:19281/launch_game

# Quit STS2
curl -s -X POST http://localhost:19281/quit_game
```

## Commands (via curl)

```bash
# Get current game state (combat/map/shop/rest/event/rewards)
curl -s http://localhost:19281/state

# Get combat-only state
curl -s http://localhost:19281/combat_state

# Play a card (hand_index required, target_index for AnyEnemy cards)
curl -s -X POST http://localhost:19281/play_card -H 'Content-Type: application/json' -d '{"hand_index": 0, "target_index": 0}'

# End turn
curl -s -X POST http://localhost:19281/end_turn

# Use potion
curl -s -X POST http://localhost:19281/use_potion -H 'Content-Type: application/json' -d '{"slot_index": 0, "target_index": 0}'

# Select cards (from pending choice)
curl -s -X POST http://localhost:19281/select_cards -H 'Content-Type: application/json' -d '{"card_indices": [0, 2]}'

# Non-combat actions
curl -s -X POST http://localhost:19281/choose_action -H 'Content-Type: application/json' -d '{"action_type":"select_node","node_index":0}'
curl -s -X POST http://localhost:19281/choose_action -H 'Content-Type: application/json' -d '{"action_type":"take_reward","reward_index":0}'
curl -s -X POST http://localhost:19281/choose_action -H 'Content-Type: application/json' -d '{"action_type":"pick_card","reward_index":2,"card_index":1}'
curl -s -X POST http://localhost:19281/choose_action -H 'Content-Type: application/json' -d '{"action_type":"rest_choice","option_index":0}'
curl -s -X POST http://localhost:19281/choose_action -H 'Content-Type: application/json' -d '{"action_type":"event_choice","option_index":1}'

# List all endpoints
curl -s http://localhost:19281/tools
```

## Play Loop

1. `curl /state` to read current game state
2. Analyze: check enemy intents, plan card order
3. Execute: `curl /play_card` for each card, respecting energy
4. `curl /end_turn` when done
5. Wait ~2 seconds, then `curl /state` again for next turn
6. Repeat until combat ends

## Decision Guidelines

- **Check enemy intents first** — prioritize blocking/killing high-damage enemies
- **Energy management** — play most impactful cards within budget
- **Card targeting** — cards with `target_type: "AnyEnemy"` need `target_index`
- **Block math** — compare incoming damage vs available block
- **Potion timing** — use in emergencies or boss fights
- **Card order** — buffs/powers before attacks to maximize damage
- **After playing a card, hand indices shift** — re-check state if unsure

## Important

- Single process handles both HTTP (19281) and WebSocket (19280)
- Game must be running with the mod loaded for commands to work
- Server must be started BEFORE entering combat (so the game mod can connect)
- Use `POST /launch_game` to start the game automatically
