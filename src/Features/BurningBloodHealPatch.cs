using System.Collections.Generic;
using ExampleMod.Core;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace ExampleMod.Features;

/// <summary>
/// Example 2: change Burning Blood's heal amount.
///
/// This patch controls the "template" dynamic var list used when a Burning Blood
/// instance creates its `DynamicVars` for the first time.
/// </summary>
[HarmonyPatch(typeof(BurningBlood), "get_CanonicalVars")]
public static class BurningBloodHealPatch
{
    /// <summary>
    /// Replaces Burning Blood's default canonical heal var with the currently configured amount.
    /// </summary>
    public static void Postfix(ref IEnumerable<DynamicVar> __result)
    {
        var healAmount = BurningBloodHealSettings.GetCurrentHealAmount();
        Log.Info($"[ExampleMod] BurningBlood.get_CanonicalVars patched to heal={healAmount}.");
        __result = new DynamicVar[] { new HealVar(healAmount) };
    }
}

/// <summary>
/// Keeps Burning Blood's runtime dynamic value synced with current settings.
///
/// Why this exists:
/// - `RelicModel.DynamicVars` is cached after first construction.
/// - If player toggles our setting later, cached `HealVar.BaseValue` would otherwise stay stale.
/// </summary>
[HarmonyPatch(typeof(RelicModel), "get_DynamicVars")]
public static class BurningBloodDynamicVarsSyncPatch
{
    /// <summary>
    /// Runs after any relic resolves its DynamicVars.
    /// If the relic is Burning Blood, we force its Heal var to current configured value.
    /// </summary>
    public static void Postfix(RelicModel __instance, DynamicVarSet __result)
    {
        if (__instance is not BurningBlood)
        {
            return;
        }

        var healAmount = BurningBloodHealSettings.GetCurrentHealAmount();
        var healVar = __result.Heal;
        if (healVar.BaseValue != healAmount)
        {
            healVar.BaseValue = healAmount;
            Log.Info($"[ExampleMod] Synced Burning Blood runtime heal to {healAmount}.");
        }
    }
}

/// <summary>
/// Shared helper for determining and logging effective Burning Blood heal amount.
/// </summary>
internal static class BurningBloodHealSettings
{
    private static decimal _lastLoggedHealAmount = -1m;

    /// <summary>
    /// Reads latest user toggle and maps it to heal amount:
    /// - enabled: 10 HP
    /// - disabled: 6 HP (vanilla value)
    /// </summary>
    public static decimal GetCurrentHealAmount()
    {
        var healAmount = FeatureSettingsStore.Current.EnableBurningBloodHeal10 ? 10m : 6m;
        if (_lastLoggedHealAmount != healAmount)
        {
            _lastLoggedHealAmount = healAmount;
            Log.Info($"[ExampleMod] Burning Blood heal amount now {healAmount}.");
        }

        return healAmount;
    }
}
