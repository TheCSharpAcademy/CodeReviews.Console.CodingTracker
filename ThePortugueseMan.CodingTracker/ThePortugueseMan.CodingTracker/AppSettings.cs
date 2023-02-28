using System.Configuration;

namespace ThePortugueseMan.CodingTracker;

internal class AppSettings
{
    public string? GetConnectionString() { return ConfigurationManager.AppSettings.Get("defaultConnectionString"); }
    public string? GetMainTableName() { return ConfigurationManager.AppSettings.Get("defaultMainTableName"); }
    public string? GetDateTimeFormatOfDB() { return ConfigurationManager.AppSettings.Get("dateTimeFormat"); }
    public string? GetTimeSpanFormatOfDB() { return ConfigurationManager.AppSettings.Get("timeSpanFormat"); }
}
