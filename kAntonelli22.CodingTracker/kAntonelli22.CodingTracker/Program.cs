using CodingTracker.DatabaseUtilities;
using Spectre.Console;

namespace CodingTracker;

class Program
{
    public static string DatabasePath { get; set; } = System.Configuration.ConfigurationManager.AppSettings.Get("DatabasePath") ?? "";
    public static string ConnectionString { get; set; } = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString") ?? "";
    static void Main(string[] args)
    {
        if (DatabasePath == "" || ConnectionString == "")
        {
            AnsiConsole.MarkupLine("[red]Database Path or Connection String not found. Please specify a path in the App configuration file.[/]");
            Environment.Exit(0);
        }

        string query = @"
            CREATE TABLE IF NOT EXISTS Sessions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Start TEXT NOT NULL,
                End TEXT NOT NULL,
                Duration TEXT NOT NULL
            );";
        DatabaseManager.RunQuery(query);
        DatabaseManager.GetSessions();
        MainMenu();
    } // end of Main Method

    static void MainMenu()
    {
        Console.Clear();
        AnsiConsole.WriteLine("Coding Tracker Main Menu\n------------------------");
        if (Output.StopwatchRunning)
            AnsiConsole.MarkupLine("   [gray]* Timer Running *[/]");
        var menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .AddChoices([
                "Exit Coding Tracker", "Start Timed Session", "End Timed Session",
                "Create New Session", "Modify Session", "Remove Session", "View Sessions"
                ]));
        AnsiConsole.WriteLine("------------------------");

        switch (menu)
        {
            case "Exit Coding Tracker":
                Environment.Exit(0);
                break;
            case "Start Timed Session":
                if (!Output.StopwatchRunning)
                    Output.StartTimed();
                else
                {
                    AnsiConsole.MarkupLine("[red]You must end your current session first[/]");
                    Output.ReturnToMenu("");
                }
                break;
            case "End Timed Session":
                if (Output.StopwatchRunning)
                    Output.EndTimed();
                else
                {
                    AnsiConsole.MarkupLine("[red]You must start a coding session first[/]");
                    Output.ReturnToMenu("");
                }
                break;
            case "Create New Session":
                Output.NewSession();
                break;
            case "Modify Session":
                if (CodingSession.Sessions.Count == 0)
                {
                    AnsiConsole.MarkupLine("[red]You don't have any sessions to modify[/]");
                    Output.ReturnToMenu("");
                }
                else
                    Output.ModifySession();
                break;
            case "Remove Session":
                if (CodingSession.Sessions.Count == 0)
                {
                    AnsiConsole.MarkupLine("[red]You don't have any sessions to remove[/]");
                    Output.ReturnToMenu("");
                }
                else
                    Output.RemoveSession();
                break;
            case "View Sessions":
                SessionViewer.ViewSessions(true, CodingSession.Sessions);
                break;
        }
        
        MainMenu();
    } // end of MainMenu Method
} // end of Program Class