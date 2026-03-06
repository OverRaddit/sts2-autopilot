using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;

namespace ExampleMod.Core;

/// <summary>
/// Central startup for the tutorial mod.
/// - Loads persisted feature toggles.
/// - Applies all Harmony patches in this assembly.
/// </summary>
public static class ModBootstrap
{
    private const string HarmonyId = "examplemod.harmony";

    private static bool _initialized;
    private static Harmony? _harmony;

    public static void Initialize()
    {
        if (_initialized)
        {
            return;
        }

        _initialized = true;

        FeatureSettingsStore.Initialize();
        Log.Info($"[ExampleMod] Loaded settings: {FeatureSettingsStore.Current}");

        _harmony = new Harmony(HarmonyId);
        _harmony.PatchAll();
        Log.Info("[ExampleMod] Harmony patches applied.");
    }
}
