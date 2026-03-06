using ExampleMod.Core;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;

namespace ExampleMod;

// STS2 discovers mods via this attribute and calls the named method once on load.
[ModInitializer(nameof(OnModLoaded))]
public static class ModEntry
{
    /// <summary>
    /// STS2 entrypoint callback invoked exactly once when the mod assembly loads.
    /// </summary>
    public static void OnModLoaded()
    {
        Log.Info("[ExampleMod] ModEntry.OnModLoaded invoked by STS2.");
        ModBootstrap.Initialize();
    }
}
