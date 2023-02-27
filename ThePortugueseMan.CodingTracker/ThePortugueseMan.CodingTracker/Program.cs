namespace ThePortugueseMan.CodingTracker;

internal class Program
{
    private static void Main(string[] args)
    {
        AppSettings appSettings = new();
        string? connectionString = appSettings.GetConnectionString();
        string? mainTableName = appSettings.GetMainTableName();

        DbCommands dbCmd = new(connectionString, mainTableName);
        Screens screen = new();

        dbCmd.Initialization();
        screen.MainMenu();

        Console.ReadLine();

    }
}