using System;
using ExampleMod.Core;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

namespace ExampleMod.Features.MainMenu;

/// <summary>
/// Adds an "Example Mod" button to main menu and shows a dialog with per-feature toggles.
/// </summary>
public static class MainMenuSettingsFeature
{
    private const string SettingsButtonName = "ExampleModSettingsButton";
    private const string SettingsDialogName = "ExampleModSettingsDialog";

    public static void AttachToMainMenu(NMainMenu mainMenu)
    {
        var buttonRoot = mainMenu.GetNodeOrNull<Control>("MainMenuTextButtons");
        if (buttonRoot == null)
        {
            Log.Warn("[ExampleMod] MainMenuTextButtons node was not found.");
            return;
        }

        if (buttonRoot.FindChild(SettingsButtonName, recursive: false, owned: false) != null)
        {
            return;
        }

        var settingsButtonReference = mainMenu.GetNodeOrNull<Control>("MainMenuTextButtons/SettingsButton");
        var button = CreateSettingsButton(mainMenu);

        if (buttonRoot is Container container)
        {
            container.AddChild(button);
            if (settingsButtonReference != null)
            {
                var targetIndex = Math.Min(settingsButtonReference.GetIndex() + 1, container.GetChildCount() - 1);
                container.MoveChild(button, targetIndex);
            }
        }
        else
        {
            buttonRoot.AddChild(button);
            if (settingsButtonReference != null)
            {
                button.Position = settingsButtonReference.Position + new Vector2(0f, settingsButtonReference.Size.Y + 10f);
            }
        }

        Log.Info("[ExampleMod] Added main-menu settings button.");
    }

    private static Button CreateSettingsButton(Node mainMenu)
    {
        var button = new Button
        {
            Name = SettingsButtonName,
            Text = "Example Mod",
            TooltipText = "Open ExampleMod feature toggles",
            CustomMinimumSize = new Vector2(220f, 40f),
            FocusMode = Control.FocusModeEnum.All,
            SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter
        };

        button.Pressed += () => OpenSettingsDialog(mainMenu);
        return button;
    }

    private static void OpenSettingsDialog(Node anchorNode)
    {
        var root = anchorNode.GetTree()?.Root;
        if (root == null)
        {
            return;
        }

        // Recreate the dialog every time so checkbox states always match persisted values.
        if (root.FindChild(SettingsDialogName, recursive: true, owned: false) is Window existing)
        {
            existing.QueueFree();
        }

        var settings = FeatureSettingsStore.Current;

        var dialog = new AcceptDialog
        {
            Name = SettingsDialogName,
            Title = "Example Mod Settings",
            DialogText = string.Empty,
            Exclusive = true
        };

        var content = new VBoxContainer
        {
            CustomMinimumSize = new Vector2(500f, 240f),
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
        };
        content.AddThemeConstantOverride("separation", 10);

        content.AddChild(new Label
        {
            Text = "Toggle each tutorial feature. Disabled features do not run.",
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            CustomMinimumSize = new Vector2(0f, 30f)
        });

        content.AddChild(CreateToggle(
            "Enable gamble button in top bar",
            settings.EnableGambleButton,
            isEnabled => FeatureSettingsStore.Update(s => s.EnableGambleButton = isEnabled)));

        content.AddChild(CreateToggle(
            "Burning Blood heals 10 HP instead of 6",
            settings.EnableBurningBloodHeal10,
            isEnabled => FeatureSettingsStore.Update(s => s.EnableBurningBloodHeal10 = isEnabled)));

        content.AddChild(CreateToggle(
            "Ironclad Strike base damage forced to 10",
            settings.EnableIroncladStrikeDamage6,
            isEnabled => FeatureSettingsStore.Update(s => s.EnableIroncladStrikeDamage6 = isEnabled)));

        content.AddChild(CreateToggle(
            "Ironclad starts each combat with +1 Strength",
            settings.EnableIroncladStartStrength,
            isEnabled => FeatureSettingsStore.Update(s => s.EnableIroncladStartStrength = isEnabled)));

        content.AddChild(new Label
        {
            Text = "Changes are applied on next relevant screen/combat.",
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            CustomMinimumSize = new Vector2(0f, 24f)
        });

        dialog.AddChild(content);
        root.AddChild(dialog);
        dialog.PopupCentered(new Vector2I(560, 330));
    }

    private static CheckBox CreateToggle(string text, bool initialValue, Action<bool> onToggled)
    {
        var checkBox = new CheckBox
        {
            Text = text,
            ButtonPressed = initialValue,
            FocusMode = Control.FocusModeEnum.All,
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
        };

        checkBox.Toggled += isEnabled => onToggled(isEnabled);
        return checkBox;
    }
}

[HarmonyPatch(typeof(NMainMenu), nameof(NMainMenu._Ready))]
public static class NMainMenuReadyPatch
{
    public static void Postfix(NMainMenu __instance)
    {
        try
        {
            MainMenuSettingsFeature.AttachToMainMenu(__instance);
        }
        catch (Exception ex)
        {
            Log.Error($"[ExampleMod] Failed to add main-menu settings button: {ex}");
        }
    }
}
