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

    public static void Initialize()
    {
        lock (LockObject)
        {
            _current = LoadInternal();
        }
    }

    public static void Update(Action<FeatureSettings> mutate)
    {
        lock (LockObject)
        {
            mutate(_current);
            SaveInternal(_current);
        }
    }

    private static FeatureSettings LoadInternal()
    {
        try
        {
            var fullPath = ResolveAbsolutePath();
            if (!File.Exists(fullPath))
            {
                return FeatureSettings.DisabledByDefault();
            }

            var json = File.ReadAllText(fullPath);
            var loaded = JsonSerializer.Deserialize<FeatureSettings>(json, JsonOptions);
            return loaded ?? FeatureSettings.DisabledByDefault();
        }
        catch (Exception ex)
        {
            Log.Error($"[ExampleMod] Failed to read settings. Using defaults. {ex}");
            return FeatureSettings.DisabledByDefault();
        }
    }

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

    private static string ResolveAbsolutePath()
    {
        return ProjectSettings.GlobalizePath(SettingsPath);
    }
}
