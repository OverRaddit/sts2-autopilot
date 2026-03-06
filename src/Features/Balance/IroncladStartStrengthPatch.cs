using System.Threading.Tasks;
using ExampleMod.Core;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Runs;

namespace ExampleMod.Features.Balance;

/// <summary>
/// Example 4: add +1 Strength to local Ironclad each combat start.
/// We append onto Hook.BeforeCombatStart's Task so this runs in normal flow.
/// </summary>
[HarmonyPatch(typeof(Hook), nameof(Hook.BeforeCombatStart))]
public static class IroncladStartStrengthPatch
{
    /// <summary>
    /// Appends our behavior after core combat-start hooks complete.
    /// </summary>
    public static void Postfix(IRunState runState, CombatState? combatState, ref Task __result)
    {
        if (!FeatureSettingsStore.Current.EnableIroncladStartStrength)
        {
            Log.Info("[ExampleMod] Start-strength feature disabled; skipping combat-start patch work.");
            return;
        }

        Log.Info("[ExampleMod] Start-strength feature enabled; appending +1 Strength task.");
        __result = ApplyAfterOriginalAsync(__result, runState, combatState);
    }

    /// <summary>
    /// Waits for base game combat-start tasks, then applies +1 Strength to local Ironclad.
    /// </summary>
    private static async Task ApplyAfterOriginalAsync(Task originalTask, IRunState runState, CombatState? combatState)
    {
        await originalTask;

        if (combatState == null)
        {
            Log.Info("[ExampleMod] CombatState was null; skipping +1 Strength.");
            return;
        }

        var localPlayer = LocalContext.GetMe(runState);
        if (localPlayer?.Character is not Ironclad)
        {
            Log.Info("[ExampleMod] Local player is not Ironclad; skipping +1 Strength.");
            return;
        }

        await PowerCmd.Apply<StrengthPower>(localPlayer.Creature, 1m, localPlayer.Creature, null);
        Log.Info("[ExampleMod] Applied +1 Strength at combat start (Ironclad).");
    }
}
