namespace CodingTracker;

using System.Configuration;

internal static class Configuration
{
    internal static string? DatabaseFilename
    {
        get
        {
            return ConfigurationManager.AppSettings.Get("DatabaseFilename");
        }
    }

    internal static string? DateTimeFormat
    {
        get
        {
            return ConfigurationManager.AppSettings.Get("DateTimeFormat");
        }
    }
}