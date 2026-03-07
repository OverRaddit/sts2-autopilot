using System.Collections.Generic;
using ExampleMod.Core;

namespace ExampleMod.Features.Settings;

/// <summary>
/// Central list of toggle rows shown in Example Mod settings.
/// </summary>
internal static class SettingsOptionCatalog
{
    /// <summary>
    /// All options in display order.
    /// </summary>
    public static IReadOnlyList<SettingsOptionDefinition> All { get; } = new[]
    {
        new SettingsOptionDefinition
        {
            Label = "Enable gamble button in top bar",
            LogKey = nameof(FeatureSettings.EnableGambleButton),
            GetValue = settings => settings.EnableGambleButton,
            SetValue = (settings, value) => settings.EnableGambleButton = value
        },
        new SettingsOptionDefinition
        {
            Label = "Burning Blood heals 10 HP instead of 6",
            LogKey = nameof(FeatureSettings.EnableBurningBloodHeal10),
            GetValue = settings => settings.EnableBurningBloodHeal10,
            SetValue = (settings, value) => settings.EnableBurningBloodHeal10 = value
        },
        new SettingsOptionDefinition
        {
            Label = "Ironclad Strike base damage forced to 10",
            LogKey = nameof(FeatureSettings.EnableIroncladStrikeDamage6),
            GetValue = settings => settings.EnableIroncladStrikeDamage6,
            SetValue = (settings, value) => settings.EnableIroncladStrikeDamage6 = value
        },
        new SettingsOptionDefinition
        {
            Label = "Ironclad starts each combat with +1 Strength",
            LogKey = nameof(FeatureSettings.EnableIroncladStartStrength),
            GetValue = settings => settings.EnableIroncladStartStrength,
            SetValue = (settings, value) => settings.EnableIroncladStartStrength = value
        }
    };
}
