namespace CodingTracker;

using System;
using System.IO;
using System.Text.Json;

public class ConfigSettings
{
    public string DbPath { get; set; } = string.Empty;
}

public static class ReadConfig
{
    public static ConfigSettings Config { get; set; }
    
    private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

    static ReadConfig()
    {
        if (!File.Exists(FilePath))
        {
            throw new FileNotFoundException($"Configuration file missing at: {FilePath}");
        }

        string jsonContent = File.ReadAllText(FilePath);

        Config = JsonSerializer.Deserialize<ConfigSettings>(jsonContent)
               ?? throw new InvalidOperationException("Failed to deserialize configuration file.");
    }
}