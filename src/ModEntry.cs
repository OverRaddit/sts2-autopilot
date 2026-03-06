using ExampleMod.Core;
using MegaCrit.Sts2.Core.Modding;

namespace ExampleMod;

// STS2 discovers mods via this attribute and calls the named method once on load.
[ModInitializer(nameof(OnModLoaded))]
public static class ModEntry
{
    public static void OnModLoaded()
    {
        ModBootstrap.Initialize();
    }
}
