namespace CodingTracker;

using System;
using System.IO;
using System.Text.Json;

public class AppSettings
{
    public string DbPath { get; set; } = string.Empty;

    private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
    
    public static AppSettings LoadConfig()
    {
        if (!File.Exists(FilePath))
        {
            throw new FileNotFoundException($"Configuration file missing at: {FilePath}");
        }

        string jsonContent = File.ReadAllText(FilePath);

        return JsonSerializer.Deserialize<AppSettings>(jsonContent)
               ?? throw new InvalidOperationException("Failed to deserialize configuration file.");
    }
    
}
