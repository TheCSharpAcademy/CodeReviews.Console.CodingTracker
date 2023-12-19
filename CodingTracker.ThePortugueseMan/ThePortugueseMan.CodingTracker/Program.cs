namespace ThePortugueseMan.CodingTracker;

internal class Program
{
    private static void Main(string[] args)
    {
        DbCommands dbCmd = new();
        
        Screens screen = new();

        dbCmd.InitializeMainTable();
        dbCmd.InitializeGoalsTable();

        screen.MainMenu();
        Console.Clear();
        Console.WriteLine("\n\nGoodbye!");
    }
}