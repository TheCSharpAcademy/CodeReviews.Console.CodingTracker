using System.Configuration;
using CodingTracker.DatabaseUtilities;
using System.Diagnostics;

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
        if (Output.stopwatchRunning)
        {
            Console.WriteLine(@"
    Coding Tracker Main Menu
    ------------------------
       * Timer Running *
    0. Exit Coding Tracker
    1. Start Timed Session
    2. End Timed Session
    3. Create New Session
    4. Modify Session
    5. Remove Session
    6. View Sessions
    ------------------------");
        }
        else
        {
            Console.WriteLine(@"
    Coding Tracker Main Menu
    ------------------------
    0. Exit Coding Tracker
    1. Start Timed Session
    2. End Timed Session
    3. Create New Session
    4. Modify Session
    5. Remove Session
    6. View Sessions
    ------------------------");
        }

        string response = InputValidator.CleanString(Console.ReadLine());

        if (response == "1")
            Output.StartTimed();
        else if (response == "2")
            if (Output.stopwatchRunning)
                Output.EndTimed();
            else
                Console.WriteLine("You must start a Coding Session first");
        else if (response == "3")
            Output.NewSession();
        else if (response == "4")
            Output.ModifySession();
        else if (response == "5")
            Output.RemoveSession();
        else if (response == "6")
            SessionViewer.ViewSessions(true, CodingSession.sessions);
        
        MainMenu();
    } // end of MainMenu Method
} // end of Program Class