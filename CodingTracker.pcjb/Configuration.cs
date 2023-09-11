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
}