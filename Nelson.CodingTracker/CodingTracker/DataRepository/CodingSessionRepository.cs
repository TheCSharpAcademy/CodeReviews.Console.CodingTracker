using System.Configuration;
using CodingTracker.ConsoleInteraction;
using CodingTracker.IDataRepository;
using CodingTracker.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace CodingTracker.DataRepository
{
    public class CodingSessionRepository : ICodingSessionRepository
    {
        readonly string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString") ?? "";
        private readonly IUserInteraction? _userInteraction;

        public CodingSessionRepository(IUserInteraction? userInteraction)
        {
            _userInteraction = userInteraction;
        }

        public void GetFromDatabase()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            const string selectQuery = @"SELECT * FROM CodingSession";

            // Use Dapper to execute the query and get a list of sessions
            var sessions = connection.Query<CodingSession>(selectQuery);

            // Display the sessions using Spectre.Console
            if (sessions.Any())
            {
                var table = new Table();
                table.AddColumn(new TableColumn("Id").RightAligned());
                table.AddColumn("StartTime");
                table.AddColumn("EndTime");
                table.AddColumn("Duration");

                foreach (var session in sessions)
                {
                    table.AddColumns(session.Id.ToString(), session.StartTime.ToString("dd-MM-yyyy"), session.EndTime.ToString("dd-MM-yyyy"), session.Duration.ToString());
                }
            }
            else
            {
                _userInteraction.ShowMessageTimeout("\n\n[Red]There are no coding sessions stored in the database.[/]");
            }
        }

        public void InsertSessionToDatabase(DateTime startTime, DateTime endTime, TimeSpan duration)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            // Define the object with properties for insertion
            var session = new CodingSession
            {
                StartTime = startTime,
                EndTime = endTime,
                Duration = duration
            };

            const string insertQuery = @"
                INSERT INTO CodingSession (StartTime, EndTime, Duration)
                VALUES (@StartTime, @EndTime, @Duration)";

            // Use Dapper to execute the query
            connection.Execute(insertQuery, session);
        }
    }
}