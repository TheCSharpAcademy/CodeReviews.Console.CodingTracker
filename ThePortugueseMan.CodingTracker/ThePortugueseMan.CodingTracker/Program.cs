namespace ThePortugueseMan.CodingTracker;

internal class Program
{
    private static void Main(string[] args)
    {
        AppSettings appSettings = new();
        DbCommands dbCmd = new();
        
        Screens screen = new();

        dbCmd.InitializeTable();
        screen.MainMenu();
        Console.Clear();
        Console.WriteLine("\n\nGoodbye!");
    }
}