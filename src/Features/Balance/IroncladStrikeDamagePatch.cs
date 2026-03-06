using System.Collections.Generic;
using ExampleMod.Core;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace ExampleMod.Features.Balance;

/// <summary>
/// Example 3: override StrikeIronclad's base damage variable.
/// </summary>
[HarmonyPatch(typeof(StrikeIronclad), "get_CanonicalVars")]
public static class IroncladStrikeDamagePatch
{
    public static void Postfix(ref IEnumerable<DynamicVar> __result)
    {
        if (!FeatureSettingsStore.Current.EnableIroncladStrikeDamage6)
        {
            return;
        }

        __result = new DynamicVar[] { new DamageVar(10m, ValueProp.Move) };
    }
}
