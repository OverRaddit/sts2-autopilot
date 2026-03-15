using ExampleMod.Automation;
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

    /// <summary>
    /// Initializes mod runtime once per process.
    /// </summary>
    public static void Initialize()
    {
        if (_initialized)
        {
            Log.Info("[ExampleMod] ModBootstrap.Initialize skipped (already initialized).");
            return;
        }

        _initialized = true;
        Log.Info("[ExampleMod] Mod bootstrap starting.");

        FeatureSettingsStore.Initialize();
        Log.Info($"[ExampleMod] Loaded settings: {FeatureSettingsStore.Current}");

        _harmony = new Harmony(HarmonyId);
        _harmony.PatchAll();
        Log.Info($"[ExampleMod] Harmony patches applied with id '{HarmonyId}'.");

        WebSocketClient.Instance.Initialize();
    }
}
