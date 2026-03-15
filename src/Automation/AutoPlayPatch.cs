using System;
using System.Threading.Tasks;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;

namespace ExampleMod.Automation;

/// <summary>
/// Harmony patch on Hook.AfterPlayerTurnStart.
/// Sends combat state to MCP server at each turn start via WebSocket.
/// Stores the PlayerChoiceContext for end_turn signaling.
/// </summary>
[HarmonyPatch(typeof(Hook), nameof(Hook.AfterPlayerTurnStart))]
public static class AfterPlayerTurnStartPatch
{
    public static void Postfix(
        CombatState combatState,
        PlayerChoiceContext choiceContext,
        Player player,
        ref Task __result)
    {
        // Only hook for the local player
        var localPlayer = LocalContext.GetMe(player.RunState);
        if (localPlayer == null || localPlayer.NetId != player.NetId)
            return;

        __result = ChainAfter(__result, combatState, choiceContext, player);
    }

    private static async Task ChainAfter(
        Task originalTask,
        CombatState combatState,
        PlayerChoiceContext choiceContext,
        Player player)
    {
        await originalTask;

        try
        {
            // Store choice context for end_turn
            CommandExecutor.SetChoiceContext(choiceContext);
            CommandExecutor.SetCombatState(combatState);
            CommandExecutor.SetPlayer(player);

            // Serialize and send state
            var json = CombatStateSerializer.Serialize(combatState, player.RunState, player);
            var message = $"{{\"type\":\"state_update\",\"data\":{json}}}";

            var sent = await WebSocketClient.Instance.SendAsync(message);
            if (sent)
            {
                Log.Info("[ExampleMod] Combat state sent to MCP server.");
            }
            else
            {
                Log.Info("[ExampleMod] MCP server not connected, state not sent.");
            }
        }
        catch (Exception ex)
        {
            Log.Info($"[ExampleMod] Error in AutoPlayPatch: {ex.Message}");
        }
    }
}
