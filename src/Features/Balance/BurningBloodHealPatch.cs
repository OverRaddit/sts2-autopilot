using System.Collections.Generic;
using ExampleMod.Core;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Relics;

namespace ExampleMod.Features.Balance;

/// <summary>
/// Example 2: change Burning Blood's heal amount by overriding its canonical dynamic vars.
/// </summary>
[HarmonyPatch(typeof(BurningBlood), "get_CanonicalVars")]
public static class BurningBloodHealPatch
{
    public static void Postfix(ref IEnumerable<DynamicVar> __result)
    {
        if (!FeatureSettingsStore.Current.EnableBurningBloodHeal10)
        {
            return;
        }

        __result = new DynamicVar[] { new HealVar(10m) };
    }
}
