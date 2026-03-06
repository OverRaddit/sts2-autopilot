using System;
using System.IO;
using System.Text.Json;
using Godot;
using MegaCrit.Sts2.Core.Logging;

namespace ExampleMod.Core;

/// <summary>
/// Reads/writes feature toggles to user data so choices persist across launches.
/// </summary>
public static class FeatureSettingsStore
{
    private const string SettingsPath = "user://example_mod_settings.json";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    private static readonly object LockObject = new();

    private static FeatureSettings _current = FeatureSettings.DisabledByDefault();

    /// <summary>
    /// Returns the in-memory settings snapshot currently used by gameplay hooks.
    /// </summary>
    public static FeatureSettings Current
    {
        get
        {
            lock (LockObject)
            {
                return _current;
            }
        }
    }

    /// <summary>
    /// Loads settings once during mod bootstrap.
    /// </summary>
    public static void Initialize()
    {
        lock (LockObject)
        {
            _current = LoadInternal();
            Log.Info($"[ExampleMod] Settings initialized from '{ResolveAbsolutePath()}': {_current}");
        }
    }

    /// <summary>
    /// Mutates the in-memory settings object and persists it to disk.
    /// </summary>
    public static void Update(Action<FeatureSettings> mutate)
    {
        lock (LockObject)
        {
            var before = _current.ToString();
            mutate(_current);
            Log.Info($"[ExampleMod] Settings updated in memory. Before='{before}' After='{_current}'");
            SaveInternal(_current);
        }
    }

    /// <summary>
    /// Reloads settings from disk and returns the refreshed settings object.
    /// Useful for gameplay hooks that must observe latest menu toggles.
    /// </summary>
    public static FeatureSettings ReloadFromDisk()
    {
        lock (LockObject)
        {
            _current = LoadInternal();
            Log.Info($"[ExampleMod] Settings reloaded from disk: {_current}");
            return _current;
        }
    }

    /// <summary>
    /// Reads settings JSON from disk, falling back to defaults if the file does not exist.
    /// </summary>
    private static FeatureSettings LoadInternal()
    {
        try
        {
            var fullPath = ResolveAbsolutePath();
            if (!File.Exists(fullPath))
            {
                Log.Info($"[ExampleMod] Settings file not found at '{fullPath}'. Using defaults.");
                return FeatureSettings.DisabledByDefault();
            }

            var json = File.ReadAllText(fullPath);
            var loaded = JsonSerializer.Deserialize<FeatureSettings>(json, JsonOptions);
            Log.Info($"[ExampleMod] Settings file read from '{fullPath}'.");
            return loaded ?? FeatureSettings.DisabledByDefault();
        }
        catch (Exception ex)
        {
            Log.Error($"[ExampleMod] Failed to read settings. Using defaults. {ex}");
            return FeatureSettings.DisabledByDefault();
        }
    }

    /// <summary>
    /// Writes current settings JSON to Godot's `user://` location.
    /// </summary>
    private static void SaveInternal(FeatureSettings settings)
    {
        try
        {
            var fullPath = ResolveAbsolutePath();
            var directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(settings, JsonOptions);
            File.WriteAllText(fullPath, json);
            Log.Info($"[ExampleMod] Settings saved: {settings}");
        }
        catch (Exception ex)
        {
            Log.Error($"[ExampleMod] Failed to save settings. {ex}");
        }
    }

    /// <summary>
    /// Converts `user://` into an absolute platform path so .NET file I/O can access it.
    /// </summary>
    private static string ResolveAbsolutePath()
    {
        return ProjectSettings.GlobalizePath(SettingsPath);
    }
}
