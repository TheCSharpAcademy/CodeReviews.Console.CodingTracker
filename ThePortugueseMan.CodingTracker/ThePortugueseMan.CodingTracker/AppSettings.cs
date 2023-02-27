using System.Configuration;

namespace ThePortugueseMan.CodingTracker;

internal class AppSettings
{
    public string? GetConnectionString() { return ConfigurationManager.AppSettings.Get("defaultConnectionString"); }
    public string? GetMainTableName() { return ConfigurationManager.AppSettings.Get("defaultMainTableName"); }
}
