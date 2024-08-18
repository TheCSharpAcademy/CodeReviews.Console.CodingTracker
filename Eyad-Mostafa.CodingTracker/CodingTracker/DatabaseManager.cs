using Microsoft.Data.Sqlite;
using Dapper;
using Microsoft.Extensions.Configuration;
using CodingTracker.Models;
using static CodingTracker.Models.TableVisualisationEngine;
using CodingTracker.Utilities;
using static CodingTracker.Utilities.UserInput;

namespace CodingTracker;

internal static class DatabaseManager
{
    static string? GetConnectionString()
    {
        var configuration =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddXmlFile("appsettings.xml", optional: false, reloadOnChange: true).Build();

        return configuration["database:connectionString"];
    }

    private static readonly string? _connectionString = GetConnectionString();

    internal static void CreataDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var sql =
            @"CREATE TABLE IF NOT EXISTS coding_sessions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                StartTime TEXT,
                EndTime TEXT,
                Duration INTEGER,
                Description TEXT
                )";
        connection.Execute(sql);
    }

    internal static void AddSession()
    {
        var date = DateTime.Today.ToString("dd/MMM/yyyy");
        Console.WriteLine(@"
Do You Want to Use Today's Date?
1- Yes
2- Enter a specific date (dd/mm/yyyy)");

        switch (Console.ReadLine()?.Trim())
        {
            case "1":
                break; 
            case "2":
                 date = GetDateInput("Please Enter the date (format: dd/mm/yyyy). Type 0 to return to main menu.\n");
                break;
            default:
                return;
        }

        string startTime = GetSessionTime("Time Format is HH:mm\n\nPlease Enter Start Time, type 0 to return to main menu");

        if (startTime == "0") return;

        string endTime = GetSessionTime("Please Enter End Time, type 0 to return to main menu");

        if(endTime == "0") return;

        int duration = CalculateDuration(startTime, endTime);

        if(Validation.CheckDuration(duration))
        {
            Console.WriteLine("Invalid Times");
            Console.WriteLine("\n\nPress any key to return to main menu....");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Please Enter a Description For This Session");
        string? description = Console.ReadLine();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var insertQuery =
            @"INSERT INTO coding_sessions (StartTime, Date, EndTime, Duration, Description) 
                VALUES (@StartTime, @Date, @EndTime, @Duration, @Description)";

        connection.Execute(insertQuery, new
        {
            StartTime = startTime,
            Date = date,
            EndTime = endTime,
            Duration = duration,
            Description = description
        });

        connection.Close();
    }

    internal static void UpdateSession()
    {
        GetSessions();

        int sessionId = GetNumberInput("\n\nPlease Enter Session Id to Update. Enter 0 to return to main menu");

        if (sessionId  == 0 ) return;

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var existingSession = connection.QueryFirstOrDefault("SELECT * FROM coding_sessions WHERE ID = @ID", new { ID = sessionId });

        if (existingSession == null)
        {
            Console.WriteLine("Session not found.");
            return;
        }

        string newDate = GetDateInput("Enter new date (dd/mm/yyyy)");
        if (newDate == "0") return;

        string newStartTime = GetSessionTime("Enter new start time (HH:mm)");
        if (newStartTime == "0") return;

        string newEndTime = GetSessionTime("Enter new end time (HH:mm)");
        if (newEndTime == "0") return;

        int newDuration = CalculateDuration(newStartTime, newEndTime);

        if (Validation.CheckDuration(newDuration))
        {
            Console.WriteLine("Invalid Times");
            Console.WriteLine("\n\nPress any key to return to main menu....");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Enter new description");
        string? newDescription = Console.ReadLine();

        var updateQuery = @"
        UPDATE coding_sessions 
        SET StartTime = @StartTime, Date = @Date, EndTime = @EndTime, Duration = @Duration, Description = @Description 
        WHERE ID = @ID";

        connection.Execute(updateQuery, new
        {
            ID = sessionId,
            StartTime = newStartTime,
            Date = newDate,
            EndTime = newEndTime,
            Duration = newDuration,
            Description =newDescription
        });

        connection.Close();
    }

    internal static void DeleteSession()
    {
        GetSessions();

        int sessionId = GetNumberInput("\n\nPlease Enter Session Id to Delete. Enter 0 to return to main menu");

        if (sessionId == 0) return;

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var deleteQuery = "DELETE FROM coding_sessions WHERE ID = @ID";
        connection.Execute(deleteQuery, new { ID = sessionId });

        Console.WriteLine($"Session {sessionId} has been deleted.");

        connection.Close();
    }

    internal static void GetSessions()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var query = 
            @"SELECT * FROM coding_sessions";
        var sessions = connection.Query<CodingSession>(query).ToList();

        foreach (var session in sessions)
        {
            var timeSpan = TimeSpan.FromMinutes(session.Duration);

            int hours = timeSpan.Hours;
            int minutes = timeSpan.Minutes;

            session.DurationFormatted = $"{hours} Hour{(hours != 1 ? "s" : "")} {minutes} Min{(minutes != 1 ? "s" : "")}";
        }

        connection.Close();
        PrintSessions(sessions);
    }

}