---
name: sts2-play
description: Play STS2 combat via MCP tools. Use when user asks to "play STS2", "play combat", "STS2 전투", "자동 플레이", or when combat state is available.
---

# STS2 Combat Auto-Play

You have access to MCP tools that let you play Slay the Spire 2 combat in real-time.

## Available Tools

- `get_combat_state` — Read the current combat state (player HP/energy/hand/enemies/intents)
- `play_card(hand_index, target_index?)` — Play a card from hand
- `end_turn` — End the current turn
- `use_potion(slot_index, target_index?)` — Use a potion

## Play Loop

1. Call `get_combat_state` to see the current state
2. Analyze the situation: player HP, energy, hand cards, enemy intents
3. Make decisions: which cards to play, in what order, whether to use potions
4. Execute actions via `play_card` / `use_potion`
5. When done playing cards, call `end_turn`
6. Wait for the next turn's state update and repeat

## Decision Guidelines

- **Check enemy intents first** — prioritize blocking/killing enemies that deal high damage
- **Energy management** — don't waste energy; play the most impactful cards within your budget
- **Card targeting** — cards with `target_type: "AnyEnemy"` need a `target_index`; others don't
- **Block math** — compare incoming damage vs available block cards
- **Potion timing** — use potions when they provide the most value (boss fights, low HP emergencies)
- **Card order matters** — play buff/power cards before attacks to maximize damage

## Example Turn

```
State: 3 energy, hand = [Strike(1), Defend(1), Bash(2), Strike(1)]
Enemy: Jaw Worm, 42 HP, intent = Attack 11

Plan: Bash(2) -> Strike(1) to maximize damage with vulnerable
Actions:
  play_card(2, 0)  # Bash targeting enemy 0
  play_card(0, 0)  # Strike targeting enemy 0 (index shifted after Bash played)
  end_turn
```

**Important**: After playing a card, hand indices shift. Always re-check `get_combat_state` if unsure about current indices.
