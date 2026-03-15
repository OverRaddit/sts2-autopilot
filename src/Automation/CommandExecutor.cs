using System;
using System.Text.Json;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;

namespace ExampleMod.Automation;

/// <summary>
/// Receives command JSON from MCP server, executes against game API,
/// and sends response back via WebSocket.
/// All methods here run on the Godot main thread (dispatched by WebSocketClient).
/// </summary>
public static class CommandExecutor
{
    private static PlayerChoiceContext? _choiceContext;
    private static CombatState? _combatState;
    private static Player? _player;

    public static void SetChoiceContext(PlayerChoiceContext ctx) => _choiceContext = ctx;
    public static void SetCombatState(CombatState cs) => _combatState = cs;
    public static void SetPlayer(Player p) => _player = p;

    /// <summary>
    /// Parse and execute a command JSON from the MCP server.
    /// Expected format: {"id": "...", "command": "play_card", "params": {...}}
    /// </summary>
    public static void HandleCommand(string json)
    {
        string? id = null;
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            id = root.GetProperty("id").GetString();
            var command = root.GetProperty("command").GetString();
            var parameters = root.GetProperty("params");

            Log.Info($"[ExampleMod] Executing command: {command} (id={id})");

            var (success, error) = command switch
            {
                "play_card" => ExecutePlayCard(parameters),
                "end_turn" => ExecuteEndTurn(),
                "use_potion" => ExecuteUsePotion(parameters),
                _ => (false, $"Unknown command: {command}")
            };

            SendResponse(id!, success, error);
        }
        catch (Exception ex)
        {
            Log.Info($"[ExampleMod] Command error: {ex.Message}");
            if (id != null)
            {
                SendResponse(id, false, ex.Message);
            }
        }
    }

    private static (bool success, string? error) ExecutePlayCard(JsonElement parameters)
    {
        if (_player?.PlayerCombatState == null || _combatState == null)
            return (false, "No active combat state");

        var handIndex = parameters.GetProperty("hand_index").GetInt32();
        var targetIndex = parameters.GetProperty("target_index").GetInt32();

        var hand = _player.PlayerCombatState.Hand.Cards;
        if (handIndex < 0 || handIndex >= hand.Count)
            return (false, $"Invalid hand_index {handIndex} (hand size: {hand.Count})");

        var card = hand[handIndex];

        // Resolve target
        Creature? target = null;
        if (targetIndex >= 0)
        {
            var enemies = _combatState.HittableEnemies;
            if (targetIndex >= enemies.Count)
                return (false, $"Invalid target_index {targetIndex} (enemies: {enemies.Count})");
            target = enemies[targetIndex];
        }

        var played = card.TryManualPlay(target);
        if (!played)
            return (false, $"Cannot play card '{card.Title}' (index {handIndex})");

        Log.Info($"[ExampleMod] Played card: {card.Title} -> {target?.Monster?.Title.GetFormattedText() ?? "no target"}");
        return (true, null);
    }

    private static (bool success, string? error) ExecuteEndTurn()
    {
        if (_player == null)
            return (false, "No player available");

        try
        {
            CombatManager.Instance.SetReadyToEndTurn(_player, false, null);
            Log.Info("[ExampleMod] Turn ended via CombatManager.SetReadyToEndTurn.");
            return (true, null);
        }
        catch (Exception ex)
        {
            Log.Info($"[ExampleMod] SetReadyToEndTurn failed: {ex.Message}");
            return (false, $"End turn failed: {ex.Message}");
        }
    }

    private static (bool success, string? error) ExecuteUsePotion(JsonElement parameters)
    {
        if (_player == null || _combatState == null)
            return (false, "No active combat state");

        var slotIndex = parameters.GetProperty("slot_index").GetInt32();
        var targetIndex = parameters.GetProperty("target_index").GetInt32();

        if (slotIndex < 0 || slotIndex >= _player.PotionSlots.Count)
            return (false, $"Invalid slot_index {slotIndex} (slots: {_player.PotionSlots.Count})");

        var potion = _player.PotionSlots[slotIndex];
        if (potion == null)
            return (false, $"No potion in slot {slotIndex}");

        // Resolve target
        Creature? target = null;
        if (targetIndex >= 0)
        {
            var enemies = _combatState.HittableEnemies;
            if (targetIndex >= enemies.Count)
                return (false, $"Invalid target_index {targetIndex} (enemies: {enemies.Count})");
            target = enemies[targetIndex];
        }

        potion.EnqueueManualUse(target);
        Log.Info($"[ExampleMod] Used potion: {potion.Title.GetFormattedText()} -> {target?.Monster?.Title.GetFormattedText() ?? "no target"}");
        return (true, null);
    }

    private static void SendResponse(string id, bool success, string? error)
    {
        var response = JsonSerializer.Serialize(new
        {
            type = "command_response",
            id,
            success,
            error
        });

        WebSocketClient.Instance.Send(response);
    }
}
