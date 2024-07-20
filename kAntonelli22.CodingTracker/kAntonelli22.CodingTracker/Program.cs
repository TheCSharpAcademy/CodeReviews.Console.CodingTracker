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
            AnsiConsole.MarkupLine("   [gray]* Timer Running *[/]");
        var menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .AddChoices(new[] {
                "Exit Coding Tracker", "Start Timed Session", "End Timed Session",
                "Create New Session", "Modify Session", "Remove Session", "View Sessions"
                }));
        AnsiConsole.WriteLine("------------------------");

        switch (menu)
        {
            case "Exit Coding Tracker":
                Environment.Exit(0);
                break;
            case "Start Timed Session":
                Output.StartTimed();
                break;
            case "End Timed Session":
                if (Output.stopwatchRunning)
                    Output.EndTimed();
                else
                {
                    AnsiConsole.MarkupLine("[red]You must start a Coding Session first[/]");
                    Output.ReturnToMenu("");
                }
                break;
            case "Create New Session":
                Output.NewSession();
                break;
            case "Modify Session":
                if (CodingSession.sessions.Count == 0)
                {
                    AnsiConsole.MarkupLine("[red]You don't have any sessions to modify[/]");
                    Output.ReturnToMenu("");
                }
                else
                    Output.ModifySession();
                break;
            case "Remove Session":
                if (CodingSession.sessions.Count == 0)
                {
                    AnsiConsole.MarkupLine("[red]You don't have any sessions to remove[/]");
                    Output.ReturnToMenu("");
                }
                else
                    Output.RemoveSession();
                break;
            case "View Sessions":
                SessionViewer.ViewSessions(true, CodingSession.sessions);
                break;
        }
        
        MainMenu();
    } // end of MainMenu Method
} // end of Program Class