using System.Configuration;
using CodingTracker.DatabaseUtilities;
using System.Diagnostics;
using Spectre.Console;

namespace CodingTracker;

class Program
{
    static Stopwatch stopwatch = new Stopwatch();
    public static string databasePath { get; set; } = ConfigurationManager.AppSettings.Get("DatabasePath") ?? "";
    public static string connectionString { get; set; } = ConfigurationManager.AppSettings.Get("ConnectionString") ?? "";
    static void Main(string[] args)
    {
        if (databasePath == "" || connectionString == "")
        {
            Console.WriteLine($"Database Path or Connection String not found. Please specify a path in the App configuration file.");
            Environment.Exit(0);
        }

        string query = @"
            CREATE TABLE IF NOT EXISTS Sessions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                start TEXT NOT NULL,
                end TEXT NOT NULL,
                duration TEXT NOT NULL
            );";
        DatabaseManager.RunQuery(query);
        DatabaseManager.GetSessions();
        MainMenu();
    } // end of Main Method

    static void MainMenu()
    {
        Console.Clear();
        AnsiConsole.WriteLine("Coding Tracker Main Menu\n------------------------");
        if (Output.stopwatchRunning)
            AnsiConsole.MarkupLine("   [blue]* Timer Running *[/]");
        var menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .AddChoices(new[] {
                "Exit Coding Tracker", "Start Timed Session", "End Timed Session",
                "Create New Session", "Modify Session", "Remove Session", "View Sessions"
                }));
        AnsiConsole.WriteLine("------------------------");
        
        if (menu == "Exit Coding Tracker")
            Environment.Exit(0);
        else if (menu == "Start Timed Session")
            Output.StartTimed();
        else if (menu == "End Timed Session")
            if (Output.stopwatchRunning)
                Output.EndTimed();
            else
            {
                AnsiConsole.MarkupLine("[red]You must start a Coding Session first[/]");
                Output.ReturnToMenu("");
            }
        else if (menu == "Create New Session")
            Output.NewSession();
        else if (menu == "Modify Session")
            Output.ModifySession();
        else if (menu == "Remove Session")
            Output.RemoveSession();
        else if (menu == "View Sessions")
            SessionViewer.ViewSessions(true, CodingSession.sessions);
        
        MainMenu();
    } // end of MainMenu Method
} // end of Program Class