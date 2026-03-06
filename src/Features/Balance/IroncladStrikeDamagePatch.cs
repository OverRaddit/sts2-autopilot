using System.Collections.Generic;
using ExampleMod.Core;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace ExampleMod.Features.Balance;

/// <summary>
/// Example 3: override StrikeIronclad's base damage variable.
/// </summary>
[HarmonyPatch(typeof(StrikeIronclad), "get_CanonicalVars")]
public static class IroncladStrikeDamagePatch
{
    /// <summary>
    /// Replaces Strike's canonical dynamic var list with one DamageVar using current toggle state.
    /// </summary>
    public static void Postfix(ref IEnumerable<DynamicVar> __result)
    {
        var damage = IroncladStrikeDamageSettings.GetCurrentDamage();
        Log.Info($"[ExampleMod] StrikeIronclad.get_CanonicalVars patched to damage={damage}.");
        __result = new DynamicVar[] { new DamageVar(damage, ValueProp.Move) };
    }
}

/// <summary>
/// Keeps Strike's runtime DynamicVars synchronized if the player flips the setting mid-session.
/// </summary>
[HarmonyPatch(typeof(CardModel), "get_DynamicVars")]
public static class IroncladStrikeDamageSyncPatch
{
    /// <summary>
    /// Forces StrikeIronclad DynamicVars.Damage.BaseValue to current setting-driven value.
    /// </summary>
    public static void Postfix(CardModel __instance, DynamicVarSet __result)
    {
        if (__instance is not StrikeIronclad)
        {
            return;
        }

        var damage = IroncladStrikeDamageSettings.GetCurrentDamage();
        var damageVar = __result.Damage;
        if (damageVar.BaseValue != damage)
        {
            damageVar.BaseValue = damage;
            Log.Info($"[ExampleMod] Synced Strike runtime base damage to {damage}.");
        }
    }
}

/// <summary>
/// Resolves and logs the active base damage value for Ironclad Strike.
/// </summary>
internal static class IroncladStrikeDamageSettings
{
    private static decimal _lastLoggedDamage = -1m;

    /// <summary>
    /// Returns 10 when enabled, otherwise vanilla 6.
    /// </summary>
    public static decimal GetCurrentDamage()
    {
        var damage = FeatureSettingsStore.Current.EnableIroncladStrikeDamage6 ? 10m : 6m;
        if (_lastLoggedDamage != damage)
        {
            _lastLoggedDamage = damage;
            Log.Info($"[ExampleMod] Ironclad Strike base damage now {damage}.");
        }

        return damage;
    }
}
