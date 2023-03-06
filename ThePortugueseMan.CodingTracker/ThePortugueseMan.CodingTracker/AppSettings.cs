using System.Configuration;

namespace ThePortugueseMan.CodingTracker;

internal class AppSettings
{
    public string? GetConnectionString() { return ConfigurationManager.AppSettings.Get("defaultConnectionString"); }
    public string? GetMainTableName() { return ConfigurationManager.AppSettings.Get("defaultMainTableName"); }
    public string? GetGoalsTableName() { return ConfigurationManager.AppSettings.Get("defaultGoalsTableName"); }
    public string? GetDateTimeDbFormat() { return ConfigurationManager.AppSettings.Get("dateTimeDbFormat"); }
    public string? GetDateTimeDisplayFormat() { return ConfigurationManager.AppSettings.Get("dateTimeDisplayFormat"); }
    public string? GetTimeSpanFormatOfDB() { return ConfigurationManager.AppSettings.Get("timeSpanFormat"); }
}
