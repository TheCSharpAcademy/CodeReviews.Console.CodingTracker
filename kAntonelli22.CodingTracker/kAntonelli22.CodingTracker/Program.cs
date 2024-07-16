using System.Configuration;
using System.Collections.Specialized;
using System.Data.SQLite;
using CodingTracker.DatabaseManager;

namespace CodingTracker;

class Program
{
    static void Main(string[] args)
    {
        string? databasePath = ConfigurationManager.AppSettings.Get("DatabasePath");
        string? connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
        if (databasePath == null || connectionString == null)
        {
            Console.WriteLine($"Database Path or Connection String not found. Please specify a path in the App configuration file.");
            Environment.Exit(0);
        }
        dbManager database = new(databasePath, connectionString);

        string query = @"
            CREATE TABLE IF NOT EXISTS Coding (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT NOT NULL,
                Time TEXT NOT NULL
            );";
        database.RunQuery(query);

        MainMenu();
    } // end of Main Method

    static void MainMenu()
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

        string response = UserInput.CleanString(Console.ReadLine());

        if (response == "1")
            Output.StartTimed();
        else if (response == "2")
            Output.EndTimed();
        else if (response == "3")
            Output.NewSession();
        else if (response == "4")
            Output.ModifySession();
        else if (response == "5")
            Output.RemoveSession();
        else if (response == "6")
            Output.ViewSessions();
        
        MainMenu();
    } // end of MainMenu Method
} // end of Program Class