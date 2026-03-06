using System;
using System.Threading.Tasks;
using ExampleMod.Core;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;

namespace ExampleMod.Features.GambleButton;

/// <summary>
/// Example 1: add a top-bar button that lets player pick a wager and run a 50/50 gamble.
/// </summary>
public static class GambleButtonFeature
{
    private const string GambleButtonName = "ExampleModGambleButton";
    private const string GambleDialogName = "ExampleModGambleDialog";
    private const decimal DefaultWagerFraction = 0.25m;

    public static void AttachToTopBar(NTopBar topBar)
    {
        var existingButton = topBar.FindChild(GambleButtonName, recursive: true, owned: false);
        if (!FeatureSettingsStore.Current.EnableGambleButton)
        {
            // If user disabled the feature in menu, remove button on next top-bar rebuild.
            existingButton?.QueueFree();
            return;
        }

        if (existingButton != null)
        {
            return;
        }

        var button = CreateGambleButton();
        var goldNode = topBar.Gold;
        var goldParent = goldNode.GetParent();

        if (goldParent is Container container)
        {
            container.AddChild(button);
            var targetIndex = Math.Min(goldNode.GetIndex() + 1, container.GetChildCount() - 1);
            container.MoveChild(button, targetIndex);
        }
        else
        {
            topBar.AddChild(button);
            button.Position = goldNode.Position + new Vector2(goldNode.Size.X + 8f, 0f);
        }

        Log.Info("[ExampleMod] Added Gamble button to top bar.");
    }

    private static Button CreateGambleButton()
    {
        var button = new Button
        {
            Name = GambleButtonName,
            Text = "Gamble",
            TooltipText = "ExampleMod: Choose your wager, then flip a 50/50 coin.",
            CustomMinimumSize = new Vector2(96f, 34f),
            SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter,
            SizeFlagsVertical = Control.SizeFlags.ShrinkCenter,
            FocusMode = Control.FocusModeEnum.All
        };

        button.Pressed += () => OnGambleButtonPressed(button);
        return button;
    }

    private static void OnGambleButtonPressed(Button sourceButton)
    {
        var runState = RunManager.Instance.DebugOnlyGetState();
        var localPlayer = LocalContext.GetMe(runState);
        if (localPlayer == null)
        {
            ShowFeedback("Gamble failed: no local player.");
            return;
        }

        if (localPlayer.Gold <= 0)
        {
            ShowFeedback("No gold to gamble.");
            return;
        }

        ShowWagerDialog(sourceButton, localPlayer.Gold);
    }

    private static void ShowWagerDialog(Button sourceButton, int currentGold)
    {
        Node? gameRoot = sourceButton.GetTree()?.Root;
        gameRoot ??= NGame.Instance;
        if (gameRoot == null)
        {
            ShowFeedback("Gamble failed: UI root unavailable.");
            return;
        }

        if (gameRoot.FindChild(GambleDialogName, recursive: true, owned: false) is ConfirmationDialog existingDialog)
        {
            existingDialog.GrabFocus();
            return;
        }

        var defaultWager = ComputeDefaultWager(currentGold);
        var wagerInput = new SpinBox
        {
            MinValue = 1,
            MaxValue = currentGold,
            Step = 1,
            Value = defaultWager,
            CustomMinimumSize = new Vector2(160f, 34f),
            SizeFlagsHorizontal = Control.SizeFlags.ShrinkEnd,
            SizeFlagsVertical = Control.SizeFlags.ShrinkCenter,
            Rounded = true
        };

        var content = new VBoxContainer
        {
            CustomMinimumSize = new Vector2(340f, 0f),
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
        };
        content.AddThemeConstantOverride("separation", 8);
        content.AddChild(new Label { Text = "Wager gold on a 50/50 coin flip.", CustomMinimumSize = new Vector2(0f, 24f) });
        content.AddChild(new Label { Text = $"Current gold: {currentGold}", CustomMinimumSize = new Vector2(0f, 24f) });

        var wagerRow = new HBoxContainer
        {
            CustomMinimumSize = new Vector2(0f, 34f),
            SizeFlagsVertical = Control.SizeFlags.ShrinkCenter
        };
        wagerRow.AddThemeConstantOverride("separation", 8);
        wagerRow.AddChild(new Label
        {
            Text = "Wager amount:",
            CustomMinimumSize = new Vector2(0f, 24f),
            SizeFlagsVertical = Control.SizeFlags.ShrinkCenter,
            VerticalAlignment = VerticalAlignment.Center
        });
        wagerRow.AddChild(wagerInput);
        content.AddChild(wagerRow);

        var dialog = new ConfirmationDialog
        {
            Name = GambleDialogName,
            Title = "ExampleMod Gamble",
            DialogText = string.Empty,
            Exclusive = true
        };

        dialog.AddChild(content);
        gameRoot.AddChild(dialog);

        dialog.Confirmed += () =>
        {
            var wager = ClampWager((int)Math.Round(wagerInput.Value), currentGold);
            dialog.QueueFree();
            TaskHelper.RunSafely(ExecuteGambleAsync(wager));
        };
        dialog.Canceled += dialog.QueueFree;
        dialog.CloseRequested += dialog.QueueFree;

        dialog.PopupCentered(new Vector2I(420, 200));
        wagerInput.CallDeferred(Control.MethodName.GrabFocus);
    }

    private static async Task ExecuteGambleAsync(int requestedWager)
    {
        var runState = RunManager.Instance.DebugOnlyGetState();
        var localPlayer = LocalContext.GetMe(runState);
        if (localPlayer == null)
        {
            ShowFeedback("Gamble failed: no local player.");
            return;
        }

        var currentGold = localPlayer.Gold;
        if (currentGold <= 0)
        {
            ShowFeedback("No gold to gamble.");
            return;
        }

        var wager = ClampWager(requestedWager, currentGold);
        var didWin = Random.Shared.Next(2) == 0;

        if (didWin)
        {
            await PlayerCmd.GainGold(wager, localPlayer);
            ShowFeedback($"WIN +{wager}g (bet {wager})");
        }
        else
        {
            await PlayerCmd.LoseGold(wager, localPlayer);
            ShowFeedback($"LOSS -{wager}g");
        }
    }

    private static int ComputeDefaultWager(int currentGold)
    {
        return ClampWager((int)Math.Floor(currentGold * (double)DefaultWagerFraction), currentGold);
    }

    private static int ClampWager(int wager, int currentGold)
    {
        wager = Math.Max(wager, 1);
        wager = Math.Min(wager, currentGold);
        return wager;
    }

    private static void ShowFeedback(string text)
    {
        var vfx = NFullscreenTextVfx.Create($"[Gamble] {text}");
        if (vfx != null)
        {
            NGame.Instance?.AddChildSafely(vfx);
        }
    }
}

[HarmonyPatch(typeof(NTopBar), nameof(NTopBar.Initialize))]
public static class NTopBarInitializeGamblePatch
{
    public static void Postfix(NTopBar __instance)
    {
        try
        {
            GambleButtonFeature.AttachToTopBar(__instance);
        }
        catch (Exception ex)
        {
            Log.Error($"[ExampleMod] Failed to add Gamble button: {ex}");
        }
    }
}
