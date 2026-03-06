using System;
using ExampleMod.Core;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace ExampleMod.Features.MainMenu;

/// <summary>
/// Adds an "Example Mod" main-menu button and opens a tutorial settings popup.
///
/// Design goals:
/// - Reuse the game's built-in modal frame (`NGenericPopup`) so visuals match native UI.
/// - Keep settings editable at runtime.
/// - Include explicit logs so learners can trace each stage.
/// </summary>
public static class MainMenuSettingsFeature
{
    private const string SettingsButtonName = "ExampleModSettingsButton";
    private const string SettingsPopupName = "ExampleModSettingsPopup";
    private const string InjectedToggleContainerName = "ExampleModToggleContainer";

    private static readonly StringName MainMenuFocusedMethod = new("MainMenuButtonFocused");
    private static readonly StringName MainMenuUnfocusedMethod = new("MainMenuButtonUnfocused");

    /// <summary>
    /// Inserts the settings button into the main menu once.
    /// </summary>
    public static void AttachToMainMenu(NMainMenu mainMenu)
    {
        var buttonRoot = mainMenu.GetNodeOrNull<Control>("MainMenuTextButtons");
        if (buttonRoot == null)
        {
            Log.Warn("[ExampleMod] MainMenuTextButtons node not found; cannot add settings button.");
            return;
        }

        if (buttonRoot.FindChild(SettingsButtonName, recursive: false, owned: false) != null)
        {
            Log.Info("[ExampleMod] Settings button already attached.");
            return;
        }

        var template = mainMenu.GetNodeOrNull<NMainMenuTextButton>("MainMenuTextButtons/SettingsButton");
        var button = CreateSettingsButton(mainMenu, template);

        if (buttonRoot is Container container)
        {
            container.AddChild(button);
            if (template != null)
            {
                var targetIndex = Math.Min(template.GetIndex() + 1, container.GetChildCount() - 1);
                container.MoveChild(button, targetIndex);
            }
        }
        else
        {
            buttonRoot.AddChild(button);
            if (template != null)
            {
                button.Position = template.Position + new Vector2(0f, template.Size.Y + 10f);
            }
        }

        Log.Info("[ExampleMod] Added main-menu Example Mod settings button.");
    }

    /// <summary>
    /// Creates a menu button using native visuals when possible.
    /// </summary>
    private static Control CreateSettingsButton(NMainMenu mainMenu, NMainMenuTextButton? template)
    {
        if (template?.Duplicate((int)(Node.DuplicateFlags.Groups | Node.DuplicateFlags.Scripts | Node.DuplicateFlags.UseInstantiation)) is NMainMenuTextButton cloned)
        {
            cloned.Name = SettingsButtonName;
            cloned.TooltipText = "Open ExampleMod tutorial settings";

            // Wire our handlers with signatures matching built-in main-menu button signals.
            cloned.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(_ => OpenSettingsPopup()));
            cloned.Connect(NClickableControl.SignalName.Focused, Callable.From<NMainMenuTextButton>(button =>
            {
                Callable.From(() => mainMenu.Call(MainMenuFocusedMethod, button)).CallDeferred();
            }));
            cloned.Connect(NClickableControl.SignalName.Unfocused, Callable.From<NMainMenuTextButton>(button =>
            {
                mainMenu.Call(MainMenuUnfocusedMethod, button);
            }));

            // Override the duplicated localization text once the node is ready.
            cloned.Connect(Node.SignalName.Ready, Callable.From(() =>
            {
                SetMainMenuButtonText(cloned, "Example Mod");
                Log.Info("[ExampleMod] Main-menu settings button text applied.");
            }));

            return cloned;
        }

        Log.Warn("[ExampleMod] Failed to clone native SettingsButton; using fallback Button.");
        var fallback = new Button
        {
            Name = SettingsButtonName,
            Text = "Example Mod",
            TooltipText = "Open ExampleMod tutorial settings",
            CustomMinimumSize = new Vector2(220f, 40f),
            FocusMode = Control.FocusModeEnum.All,
            SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter
        };

        fallback.Pressed += OpenSettingsPopup;
        return fallback;
    }

    /// <summary>
    /// Writes custom text into the duplicated native menu button label.
    /// </summary>
    private static void SetMainMenuButtonText(NMainMenuTextButton button, string text)
    {
        if (button.label != null)
        {
            button.label.Text = text;
            return;
        }

        if (FindFirstMegaLabel(button) is { } nestedLabel)
        {
            nestedLabel.Text = text;
        }
    }

    /// <summary>
    /// Recursively scans descendants to find the first MegaLabel.
    /// </summary>
    private static MegaLabel? FindFirstMegaLabel(Node node)
    {
        if (node is MegaLabel label)
        {
            return label;
        }

        foreach (Node child in node.GetChildren())
        {
            if (FindFirstMegaLabel(child) is { } found)
            {
                return found;
            }
        }

        return null;
    }

    /// <summary>
    /// Opens the native-styled popup and injects tutorial checkboxes into the popup body area.
    /// </summary>
    private static void OpenSettingsPopup()
    {
        Log.Info("[ExampleMod] Open settings popup requested.");
        var modalContainer = NModalContainer.Instance;
        if (modalContainer == null)
        {
            Log.Warn("[ExampleMod] NModalContainer instance unavailable; cannot open settings popup.");
            return;
        }

        if (modalContainer.OpenModal is NGenericPopup openPopup && openPopup.Name == SettingsPopupName)
        {
            Log.Info("[ExampleMod] Settings popup already open.");
            return;
        }

        if (modalContainer.FindChild(SettingsPopupName, recursive: false, owned: false) is NGenericPopup existingPopup)
        {
            Log.Info("[ExampleMod] Reusing existing settings popup node.");
            existingPopup.GrabFocus();
            return;
        }

        var popup = NGenericPopup.Create();
        if (popup == null)
        {
            Log.Error("[ExampleMod] Failed to create NGenericPopup for settings.");
            return;
        }

        popup.Name = SettingsPopupName;
        popup.Connect(Node.SignalName.Ready, Callable.From(() => ConfigureSettingsPopup(popup)));
        modalContainer.Add(popup);
        Log.Info("[ExampleMod] Settings popup added to modal container.");
    }

    /// <summary>
    /// Configures popup text/buttons and mounts toggle controls inside the popup body.
    /// </summary>
    private static void ConfigureSettingsPopup(NGenericPopup popup)
    {
        var verticalPopup = popup.GetNodeOrNull<NVerticalPopup>("VerticalPopup");
        if (verticalPopup == null)
        {
            Log.Error("[ExampleMod] VerticalPopup node missing in settings popup.");
            return;
        }

        Log.Info("[ExampleMod] Configuring Example Mod popup UI.");
        verticalPopup.SetText("Example Mod", "Toggle tutorial examples. Changes apply immediately.");
        verticalPopup.InitYesButton(new LocString("main_menu_ui", "GENERIC_POPUP.confirm"), _ =>
        {
            Log.Info("[ExampleMod] Settings popup closed with confirm.");
        });
        verticalPopup.YesButton.SetText("Close");
        verticalPopup.HideNoButton();

        InjectToggleControls(verticalPopup);
    }

    /// <summary>
    /// Inserts the checkbox list in the same area used by the popup body label.
    /// </summary>
    private static void InjectToggleControls(NVerticalPopup verticalPopup)
    {
        if (verticalPopup.FindChild(InjectedToggleContainerName, recursive: false, owned: false) is Control existing)
        {
            existing.QueueFree();
        }

        // Pull latest settings from disk so users always see persisted values on open.
        FeatureSettingsStore.ReloadFromDisk();
        var settings = FeatureSettingsStore.Current;

        var content = new VBoxContainer
        {
            Name = InjectedToggleContainerName,
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
            SizeFlagsVertical = Control.SizeFlags.ExpandFill
        };
        content.AddThemeConstantOverride("separation", 4);

        content.AddChild(CreateToggle(
            "Enable gamble button in top bar",
            settings.EnableGambleButton,
            isEnabled =>
            {
                FeatureSettingsStore.Update(s => s.EnableGambleButton = isEnabled);
                Log.Info($"[ExampleMod] Toggle changed: EnableGambleButton={isEnabled}");
            }));

        content.AddChild(CreateToggle(
            "Burning Blood heals 10 HP instead of 6",
            settings.EnableBurningBloodHeal10,
            isEnabled =>
            {
                FeatureSettingsStore.Update(s => s.EnableBurningBloodHeal10 = isEnabled);
                Log.Info($"[ExampleMod] Toggle changed: EnableBurningBloodHeal10={isEnabled}");
            }));

        content.AddChild(CreateToggle(
            "Ironclad Strike base damage forced to 10",
            settings.EnableIroncladStrikeDamage6,
            isEnabled =>
            {
                FeatureSettingsStore.Update(s => s.EnableIroncladStrikeDamage6 = isEnabled);
                Log.Info($"[ExampleMod] Toggle changed: EnableIroncladStrikeDamage6={isEnabled}");
            }));

        content.AddChild(CreateToggle(
            "Ironclad starts each combat with +1 Strength",
            settings.EnableIroncladStartStrength,
            isEnabled =>
            {
                FeatureSettingsStore.Update(s => s.EnableIroncladStartStrength = isEnabled);
                Log.Info($"[ExampleMod] Toggle changed: EnableIroncladStartStrength={isEnabled}");
            }));

        if (verticalPopup.GetNodeOrNull<Control>("Description") is { } description)
        {
            description.Visible = false;
            content.Position = description.Position;
            content.Size = description.Size;
        }
        else
        {
            // Fallback geometry if game-side popup hierarchy changes in future builds.
            content.Position = new Vector2(70f, 170f);
            content.Size = new Vector2(540f, 220f);
            Log.Warn("[ExampleMod] Description node missing; using fallback toggle container bounds.");
        }

        verticalPopup.AddChild(content);
    }

    /// <summary>
    /// Creates one checkbox row and wires it to a specific persisted feature toggle.
    /// </summary>
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

/// <summary>
/// Hooks into main-menu `_Ready` so the Example Mod button appears every launch.
/// </summary>
[HarmonyPatch(typeof(NMainMenu), nameof(NMainMenu._Ready))]
public static class NMainMenuReadyPatch
{
    /// <summary>
    /// Adds the button after base menu setup has created all stock controls.
    /// </summary>
    public static void Postfix(NMainMenu __instance)
    {
        try
        {
            Log.Info("[ExampleMod] NMainMenu._Ready postfix running.");
            MainMenuSettingsFeature.AttachToMainMenu(__instance);
        }
        catch (Exception ex)
        {
            Log.Error($"[ExampleMod] Failed to add main-menu settings button: {ex}");
        }
    }
}
