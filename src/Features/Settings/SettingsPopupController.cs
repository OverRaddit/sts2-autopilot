using System;
using ExampleMod.Core;
using Godot;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;

namespace ExampleMod.Features.Settings;

/// <summary>
/// Owns popup lifecycle and settings-toggle content rendering.
/// </summary>
internal static class SettingsPopupController
{
    /// <summary>
    /// Opens Example Mod settings popup in the game's native modal container.
    /// </summary>
    public static void Open()
    {
        Log.Info("[ExampleMod] Open settings popup requested.");
        var modalContainer = NModalContainer.Instance;
        if (modalContainer == null)
        {
            Log.Warn("[ExampleMod] NModalContainer instance unavailable; cannot open settings popup.");
            return;
        }

        if (modalContainer.OpenModal is NGenericPopup openPopup && openPopup.Name == SettingsConstants.SettingsPopupName)
        {
            Log.Info("[ExampleMod] Settings popup already open.");
            return;
        }

        if (modalContainer.FindChild(SettingsConstants.SettingsPopupName, recursive: false, owned: false) is NGenericPopup existingPopup)
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

        popup.Name = SettingsConstants.SettingsPopupName;
        popup.Connect(Node.SignalName.Ready, Callable.From(() => ConfigurePopup(popup)));
        modalContainer.Add(popup);
        Log.Info("[ExampleMod] Settings popup added to modal container.");
    }

    /// <summary>
    /// Configures popup text and buttons, then injects settings toggle rows.
    /// </summary>
    private static void ConfigurePopup(NGenericPopup popup)
    {
        var verticalPopup = popup.GetNodeOrNull<NVerticalPopup>("VerticalPopup");
        if (verticalPopup == null)
        {
            Log.Error("[ExampleMod] VerticalPopup node missing in settings popup.");
            return;
        }

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
    /// Adds one checkbox row per feature setting into the popup body area.
    /// </summary>
    private static void InjectToggleControls(NVerticalPopup verticalPopup)
    {
        if (verticalPopup.FindChild(SettingsConstants.InjectedToggleContainerName, recursive: false, owned: false) is Control existing)
        {
            existing.QueueFree();
        }

        FeatureSettingsStore.ReloadFromDisk();
        var settings = FeatureSettingsStore.Current;

        var content = new VBoxContainer
        {
            Name = SettingsConstants.InjectedToggleContainerName,
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
            SizeFlagsVertical = Control.SizeFlags.ExpandFill
        };
        content.AddThemeConstantOverride("separation", 4);

        foreach (var option in SettingsOptionCatalog.All)
        {
            content.AddChild(CreateToggle(
                option.Label,
                option.GetValue(settings),
                isEnabled =>
                {
                    FeatureSettingsStore.Update(current => option.SetValue(current, isEnabled));
                    Log.Info($"[ExampleMod] Toggle changed: {option.LogKey}={isEnabled}");
                }));
        }

        if (verticalPopup.GetNodeOrNull<Control>("Description") is { } description)
        {
            description.Visible = false;
            content.Position = description.Position;
            content.Size = description.Size;
        }
        else
        {
            content.Position = new Vector2(70f, 170f);
            content.Size = new Vector2(540f, 220f);
            Log.Warn("[ExampleMod] Description node missing; using fallback toggle container bounds.");
        }

        verticalPopup.AddChild(content);
    }

    /// <summary>
    /// Creates one checkbox row bound to a settings update callback.
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
