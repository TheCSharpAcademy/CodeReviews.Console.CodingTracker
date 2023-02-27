using System.Configuration;
using ConsoleTableExt;


namespace ThePortugueseMan.CodingTracker;

internal class Program
{
    private static void Main(string[] args)
    {
        string? connectionString = ConfigurationManager.AppSettings.Get("defaultConnectionString");
        string? mainTableName = ConfigurationManager.AppSettings.Get("defaultMainTableName");
        DbCommands dbCmd = new(connectionString, mainTableName);

        dbCmd.Initialization();

        Console.ReadLine();

    }
}