using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.Sqlite;
using Dapper;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using System.Diagnostics;
using System.Globalization;
using System.Collections;

namespace coding_tracker;

public class SessionController
{
    public UserInput UserInput { get; set; }

    static int id = 0;
    static string date = "";
    static string startTime = "";
    static string endTime = "";
    static string duration = "";

    CodingSession session = new()
    {
        Id = id,
        StartTime = startTime,
        EndTime = endTime,
        Duration = duration,
    };


    readonly string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;
    TableVisualisationEngine tableGeneration = new();
    internal void AddNewManualSession()
    {
        string[] sessionTime = UserInput.ManualSessionInput();

        session.StartTime = sessionTime[0];
        session.EndTime = sessionTime[1];
        session.Duration = sessionTime[2];

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = "INSERT INTO Coding_Session (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)";
            connection.Execute(sqlQuery, new {session.StartTime, session.EndTime, session.Duration});
        }
    }

    internal void ViewAllSessions()
    {
        using (var connection = new  SqliteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = "SELECT Id, strftime('%d-%m-%Y',StartTime) AS Date, strftime('%H:%M:%S',StartTime) AS StartTime, strftime('%H:%M:%S',EndTime) AS EndTime, Duration FROM Coding_Session";
            var codingSessions = connection.Query(sqlQuery);

            List<string> columnHeaders = new List<string>() { "Id","Date","Start Time","End Time","Duration"};
            List<string> rowData = new();

            foreach (var codingSession in codingSessions)
            {
                rowData.Add(Convert.ToString(codingSession.Id));
                rowData.Add(codingSession.Date);
                rowData.Add(codingSession.StartTime);
                rowData.Add(codingSession.EndTime);
                rowData.Add(SecondsConversion(codingSession.Duration));
            }
            tableGeneration.TableGeneration(columnHeaders, rowData);
        }

        
    }

    internal string SecondsConversion(string secondsDuration)
    {
        int totalseconds = Convert.ToInt32(secondsDuration);
        int seconds = totalseconds % 60;
        int minutes = (totalseconds % 3600) / 60;
        int hours = totalseconds / 3600;
        string totalDuration = "";

        if(hours > 0)
        {
            return totalDuration = String.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
        else if (minutes > 0)
        {
            return totalDuration = String.Format("00:{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            return totalDuration = String.Format("00:00:{0:00}", seconds);
        }
    }

    internal int[] GetIds()
    {
        List<int> rowData = new();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = "SELECT * FROM Coding_Session";
            var codingSessions = connection.Query(sqlQuery);

            foreach (var codingSession in codingSessions)
            {
                rowData.Add(Convert.ToInt32(codingSession.Id));
            }
        }
        return rowData.ToArray();


    }

    internal void UpdateSession(int idSelection)
    {
        string[] sessionTime = UserInput.ManualSessionInput();
        session.Id = idSelection;
        session.StartTime = sessionTime[0];
        session.EndTime = sessionTime[1];
        session.Duration = sessionTime[2];


        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = 
                @"UPDATE Coding_Session SET
                StartTime = @StartTime,
                EndTime = @EndTime,
                Duration = @Duration
                Where Id = @Id";

            connection.Execute(sqlQuery, new {session.Id, session.StartTime, session.EndTime, session.Duration });
        }
    }

    internal void DeleteSession(int idSelection)
    {
        session.Id = idSelection;

        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = @"DELETE FROM Coding_Session WHERE Id = @Id";

            connection.Execute(sqlQuery, new {session.Id});
        }
    }

    internal void SessionStopwatch()
    {
        string[] dateTime = new string[3];
        Stopwatch stopwatch = new();
        stopwatch.Start();
        dateTime[0] = GetTimeStamp();
        tableGeneration.StopwatchTable();
        while (true)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(Convert.ToInt32(stopwatch.Elapsed.TotalSeconds));
            Console.SetCursorPosition(30, 1);
            AnsiConsole.Markup($"[teal]{timeSpan.ToString("c")}[/]");
            Console.WriteLine("\r");
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.F) break;

        }
        dateTime[1] = GetTimeStamp();
        int timer = Convert.ToInt32(stopwatch.Elapsed.TotalSeconds);
        dateTime[2] = timer.ToString();

        session.StartTime = dateTime[0];
        session.EndTime = dateTime[1];
        session.Duration = dateTime[2];
        AnsiConsole.Clear();

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = "INSERT INTO Coding_Session (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)";
            connection.Execute(sqlQuery, new { session.StartTime, session.EndTime, session.Duration });
        }

    }

    internal string GetTimeStamp()
    {
        DateTime timeStam = DateTime.Now;
        return timeStam.ToString("yyyy-MM-dd HH:mm:ss");
    }

    internal void WeeklyCodingSessions()
    {
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = @"SELECT strftime('%W', StartTime) AS WeekNumber, SUM(Duration) AS Duration From Coding_Session GROUP BY strftime('%W', StartTime)";
            var codingSessions = connection.Query(sqlQuery);

            List<string> rowData = new();

            foreach (var codingSession in codingSessions)
            {
                string parsedTime = SecondsConversion(Convert.ToString(codingSession.Duration));
                rowData.Add(codingSession.WeekNumber);
                rowData.Add(parsedTime);

            }


            tableGeneration.WeekGenerator(rowData);
        }
    }

    
    
}