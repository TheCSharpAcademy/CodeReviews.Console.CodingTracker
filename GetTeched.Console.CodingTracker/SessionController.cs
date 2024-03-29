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

namespace coding_tracker;

public class SessionController
{
    public UserInput UserInput { get; set; }

    readonly string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;
    TableVisualisationEngine tableGeneration = new();
    internal void AddNewManualEntry()
    {
        string[] sessionTime = UserInput.ManualSessionInput();
        int id = 0;
        string startTime = sessionTime[0];
        string endTime = sessionTime[1];
        string duration = sessionTime[2];

        CodingSession session = new()
        {
            Id = id,
            StartTime = startTime,
            EndTime = endTime,
            Duration = duration
        };

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
            string sqlQuery = "SELECT * FROM Coding_Session";
            var codingSessions = connection.Query(sqlQuery);

            List<string> columnHeaders = new List<string>() { "Id","Start Time","End Time","Duration"};
            List<string> rowData = new();

            foreach (var codingSession in codingSessions)
            {
                rowData.Add(Convert.ToString(codingSession.Id));
                rowData.Add(codingSession.StartTime);
                rowData.Add(codingSession.EndTime);
                rowData.Add(codingSession.Duration);
            }
            tableGeneration.TableGeneration(columnHeaders, rowData);
        }

        
    }

    internal string[] SelectionViewSessions()
    {
        List<string> rowData = new();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = "SELECT * FROM Coding_Session";
            var codingSessions = connection.Query(sqlQuery);

            foreach (var codingSession in codingSessions)
            {
                rowData.Add(Convert.ToString(codingSession.Id));

            }
        }
        return rowData.ToArray();


    }

    internal void UpdateSession(string idSelection)
    {
        string[] sessionTime = UserInput.ManualSessionInput();
        int id = Convert.ToInt32(idSelection);
        string startTime = sessionTime[0];
        string endTime = sessionTime[1];
        string duration = sessionTime[2];

        CodingSession session = new()
        {
            Id = id,
            StartTime = startTime,
            EndTime = endTime,
            Duration = duration
        };

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

}