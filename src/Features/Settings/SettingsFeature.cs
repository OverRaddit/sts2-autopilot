using System;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

namespace ExampleMod.Features.Settings;

/// <summary>
/// Facade for Example Mod settings entrypoint in the main menu.
/// </summary>
public static class SettingsFeature
{
    /// <summary>
    /// Injects the Example Mod settings button into main menu.
    /// </summary>
    public static void AttachToMainMenu(NMainMenu mainMenu)
    {
        SettingsButtonInstaller.Attach(mainMenu, SettingsPopupController.Open);
    }
}

/// <summary>
/// Hooks into main-menu setup so the settings button appears every launch.
/// </summary>
[HarmonyPatch(typeof(NMainMenu), nameof(NMainMenu._Ready))]
public static class NMainMenuReadySettingsPatch
{
    /// <summary>
    /// Adds button after base main menu setup has finished building stock controls.
    /// </summary>
    public static void Postfix(NMainMenu __instance)
    {
        try
        {
            Log.Info("[ExampleMod] NMainMenu._Ready postfix running.");
            SettingsFeature.AttachToMainMenu(__instance);
        }
        catch (Exception ex)
        {
            Log.Error($"[ExampleMod] Failed to add main-menu settings button: {ex}");
        }
    }
}
