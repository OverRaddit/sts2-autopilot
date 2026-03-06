namespace ExampleMod.Core;

/// <summary>
/// Toggle set controlled from the main-menu "Example Mod" button.
/// Everything is off by default so users explicitly opt in.
/// </summary>
public sealed class FeatureSettings
{
    public bool EnableGambleButton { get; set; }

    public bool EnableBurningBloodHeal10 { get; set; }

    public bool EnableIroncladStrikeDamage6 { get; set; }

    public bool EnableIroncladStartStrength { get; set; }

    public static FeatureSettings DisabledByDefault()
    {
        return new FeatureSettings
        {
            EnableGambleButton = false,
            EnableBurningBloodHeal10 = false,
            EnableIroncladStrikeDamage6 = false,
            EnableIroncladStartStrength = false
        };
    }

    public override string ToString()
    {
        return $"Gamble={EnableGambleButton}, BurningBlood10={EnableBurningBloodHeal10}, Strike10={EnableIroncladStrikeDamage6}, StartStrength={EnableIroncladStartStrength}";
    }
}
