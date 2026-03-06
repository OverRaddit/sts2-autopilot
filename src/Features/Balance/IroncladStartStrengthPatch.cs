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
    public static void Postfix(IRunState runState, CombatState? combatState, ref Task __result)
    {
        if (!FeatureSettingsStore.Current.EnableIroncladStartStrength)
        {
            return;
        }

        __result = ApplyAfterOriginalAsync(__result, runState, combatState);
    }

    private static async Task ApplyAfterOriginalAsync(Task originalTask, IRunState runState, CombatState? combatState)
    {
        await originalTask;

        if (combatState == null)
        {
            return;
        }

        var localPlayer = LocalContext.GetMe(runState);
        if (localPlayer?.Character is not Ironclad)
        {
            return;
        }

        await PowerCmd.Apply<StrengthPower>(localPlayer.Creature, 1m, localPlayer.Creature, null);
        Log.Info("[ExampleMod] Applied +1 Strength at combat start (Ironclad). ");
    }
}
