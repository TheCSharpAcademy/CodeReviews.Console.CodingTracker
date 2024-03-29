using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.Sqlite;
using Dapper;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding_tracker;

public class SessionController
{
    public UserInput UserInput { get; set; }

    readonly string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;

    internal void AddNewEntry()
    {
        string[] sessionTime = UserInput.GetSessionInput();
        string startTime = sessionTime[0];
        string endTime = sessionTime[1];
        string duration = sessionTime[2];

        CodingSession session = new()
        {
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

}
