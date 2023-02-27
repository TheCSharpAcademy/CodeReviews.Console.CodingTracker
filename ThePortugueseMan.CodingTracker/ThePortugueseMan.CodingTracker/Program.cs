using System.Configuration;
using System.Collections.Specialized;
using System.Data.Common;

namespace ThePortugueseMan.CodingTracker;

internal class Program
{
    private static void Main(string[] args)
    {
        string? connectionString = ConfigurationManager.AppSettings.Get("defaultConnectionString");
        DbCommands dbCmd = new(connectionString);

        dbCmd.Initialization();

    }
}