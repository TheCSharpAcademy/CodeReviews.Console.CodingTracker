using System.Configuration;

namespace ThePortugueseMan.CodingTracker;

internal class AppSettings
{
    public string? GetConnectionString() { return ConfigurationManager.AppSettings.Get("defaultConnectionString"); }
    
    public string? GetMainTableName() { return ConfigurationManager.AppSettings.Get("defaultMainTableName"); }
    
    public string? GetGoalsTableName() { return ConfigurationManager.AppSettings.Get("defaultGoalsTableName"); }
    
    public string? GetDateTimeMainDbFormat() { return ConfigurationManager.AppSettings.Get("dateTimeMainDbFormat"); }
    
    public string? GetDateTimeGoalsDbFormat() { return ConfigurationManager.AppSettings.Get("dateTimeGoalsDbFormat"); }
    
    public string? GetDateTimeDisplayFormat() { return ConfigurationManager.AppSettings.Get("dateTimeDisplayFormat"); }
    
    public string? GetTimeSpanFormatOfDB() { return ConfigurationManager.AppSettings.Get("timeSpanFormat"); }
}
