using System.Configuration;
using System.Data.Common;
using System.Globalization;
using codingTracker.Fennikko.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

public class CodingController
{
    private static readonly string ConnectionString = ConfigurationManager.AppSettings.Get("connectionString") +
                                                      ConfigurationManager.AppSettings.Get("databasePath");

    public static void DatabaseCreation()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Execute(
            """
            CREATE TABLE IF NOT EXISTS coding_tracker (
                 Id INTEGER PRIMARY KEY AUTOINCREMENT,
                 StartTime TEXT,
                 EndTime TEXT,
                 Duration TEXT
                 )               
            """);
    }

    public static void Session()
    {
        var startTime = Validation.GetDateInput("Please insert the date and time for your start time:",
            "(Format dd-mm-yy HH:mm)", "Or type 0 to return to the main menu");
        var endTime = Validation.GetDateInput("Please insert the date and time for your end time:",
            "(Format dd-mm-yy HH:mm)", "Or type 0 to return to the main menu");
        var startDateTime = DateTime.ParseExact(startTime, "dd-MM-yy HH:mm", new CultureInfo("en-US"));
        var endDateTime = DateTime.ParseExact(endTime, "dd-MM-yy HH:mm", new CultureInfo("en-US"));
        while (startDateTime > endDateTime)
        {
            startTime = Validation.GetDateInput("Start time is after end time, please enter valid start time:",
                "(Format dd-mm-yy HH:mm)", "Or type 0 to return to main menu");
            startDateTime = DateTime.ParseExact(startTime, "dd-MM-yy HH:mm", new CultureInfo("en-US"));
        }
        using var connection = new SqliteConnection(ConnectionString);
        var command = "INSERT INTO coding_tracker (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)";
        var session = new CodingSession { StartTime = startTime, EndTime = endTime };
        var sessionCreation = connection.Execute(command, session);
        AnsiConsole.Write(new Markup($"[green]{sessionCreation}[/] session added"));
    }

    public static void GetAllSessions()
    {

    }

    public static void TableCreation(IEnumerable<CodingSession> sessions)
    {

    }
}