using System;
using ExampleMod.Core;

namespace ExampleMod.Features.Settings;

/// <summary>
/// Describes one toggle row rendered in the settings popup.
/// </summary>
internal sealed class SettingsOptionDefinition
{
    /// <summary>
    /// Row label shown next to the checkbox.
    /// </summary>
    public required string Label { get; init; }

    /// <summary>
    /// Short key used in logs when this setting changes.
    /// </summary>
    public required string LogKey { get; init; }

    /// <summary>
    /// Reads setting value from the current settings snapshot.
    /// </summary>
    public required Func<FeatureSettings, bool> GetValue { get; init; }

    /// <summary>
    /// Writes setting value to the persistent settings object.
    /// </summary>
    public required Action<FeatureSettings, bool> SetValue { get; init; }
}
