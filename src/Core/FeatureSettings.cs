namespace ExampleMod.Core;

/// <summary>
/// Toggle set controlled from the main-menu "Example Mod" button.
/// Most features are off by default, except Burning Blood 10 HP (on by default).
/// </summary>
public sealed class FeatureSettings
{
    /// <summary>
    /// Example 1 toggle: show/hide gamble button on the top bar.
    /// </summary>
    public bool EnableGambleButton { get; set; }

    /// <summary>
    /// Example 2 toggle: make Burning Blood heal 10 instead of 6.
    /// </summary>
    public bool EnableBurningBloodHeal10 { get; set; }

    /// <summary>
    /// Example 3 toggle: force Ironclad Strike base damage to 10.
    /// (Property name keeps legacy suffix for backward compatibility with older settings files.)
    /// </summary>
    public bool EnableIroncladStrikeDamage6 { get; set; }

    /// <summary>
    /// Example 4 toggle: grant +1 Strength at start of each Ironclad combat.
    /// </summary>
    public bool EnableIroncladStartStrength { get; set; }

    /// <summary>
    /// Creates default settings for first launch.
    /// </summary>
    public static FeatureSettings DisabledByDefault()
    {
        return new FeatureSettings
        {
            EnableGambleButton = false,
            EnableBurningBloodHeal10 = true,
            EnableIroncladStrikeDamage6 = false,
            EnableIroncladStartStrength = false
        };
    }

    public override string ToString()
    {
        return $"Gamble={EnableGambleButton}, BurningBlood10={EnableBurningBloodHeal10}, Strike10={EnableIroncladStrikeDamage6}, StartStrength={EnableIroncladStartStrength}";
    }
}
