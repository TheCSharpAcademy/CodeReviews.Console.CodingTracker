using System.Globalization;
using CodingTracker.ConsoleInteraction;
using CodingTracker.IDataRepository;
using CodingTracker.Models;
using CodingTracker.Utilities;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace CodingTracker.DataRepository
{
    public class CodingSessionRepository : ICodingSessionRepository
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString") ?? "";
        private readonly IUserInteraction? _userInteraction;
        private readonly IUtils _utils;

        public CodingSessionRepository(IUserInteraction? userInteraction, IUtils utils)
        {
            _userInteraction = userInteraction;
            _utils = utils;
        }

        public void DeleteSessionFromDatabase(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            // Get all sessions from the database
            const string selectQuery = @"SELECT * FROM CodingSession";
            var sessions = connection.Query<CodingSession>(selectQuery).ToList();

            // Check if the index is valid
            if (id < 1 || id > sessions.Count)
            {
                _userInteraction.ShowMessageTimeout($"\n\n[Red]Id: {id} doesn't exist.[/]");
                return;
            }

            // Get the Id of the session to delete
            var sessionToDelete = sessions[id - 1];
            var idToDelete = sessionToDelete.Id;
            _userInteraction.ShowMessageTimeout($"\n\n[Yellow]Deleting Coding Session with Id: {id}...[/]");

            // Delete the session from the database
            const string deleteQuery = @"DELETE FROM CodingSession WHERE Id = @Id";

            int rowCount = connection.Execute(deleteQuery, new {Id = idToDelete});

            if (rowCount == 0)
            {
                _userInteraction.ShowMessageTimeout($"\n\n[Red]There is no coding session with Id: {id}.[/]");
            }
            else
            {
                _userInteraction.ShowMessageTimeout($"\n\n[Green]Coding session with Id: {id}, has been deleted.[/]");
            }
        }

        public void GetFromDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            const string selectQuery = @"SELECT * FROM CodingSession";

            // Use Dapper to execute the query and get a list of sessions
            var sessions = connection.Query<CodingSession>(selectQuery).ToList();

            // Display the sessions using Spectre.Console
            if (sessions.Count != 0)
            {
                var table = new Table
                {
                    Border = TableBorder.Double
                };
                table.AddColumn(new TableColumn("Id").RightAligned());
                table.AddColumn("StartTime");
                table.AddColumn("EndTime");
                table.AddColumn("Duration");

                for (int i = 0; i < sessions.Count; i++)
                {
                    var session = sessions[i];
                    table.AddRow(
                        (i + 1).ToString(),
                        session.StartTime.ToString("dd-MM-yyyy HH:mm:ss"),
                        session.EndTime.ToString("dd-MM-yyyy HH:mm:ss"),
                        session.Duration
                    );

                    // Add a separator rule after each row except the last one
                    if (i < sessions.Count - 1)
                    {
                        // Add an empty row to separate with an empty line
                        table.AddEmptyRow();
                    }
                }
                _userInteraction.ShowMessage(table);
            }
            else
            {
                _userInteraction.ShowMessageTimeout("\n\n[Red]There are no coding sessions stored in the database.[/]");
            }
        }

        public List<CodingSession> GetAllFromDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            const string selectQuery = @"SELECT * FROM CodingSession";

            // Use Dapper to execute the query and get a list of sessions
            var sessions = connection.Query<CodingSession>(selectQuery);

            return sessions.ToList();
        }

        public void InsertSessionToDatabase(DateTime startTime, DateTime endTime, string duration)
        {
            using var connection = new SqliteConnection(_connectionString);
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

            _userInteraction.ShowMessageTimeout($"\n\n[Green]Coding session, has been inserted.[/]");
        }

        public void UpdateEndTimeInDatabase(int id, DateTime endTime)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            const string selectQuery = @"SELECT * FROM CodingSession";

            // Use Dapper to execute the query and get a list of sessions
            var sessions = connection.Query<CodingSession>(selectQuery).ToList();

            var sessionToUpdate = sessions[id - 1];
            var idToUpdate = sessionToUpdate.Id;

            string checkQuery = "SELECT EXISTS(SELECT 1 From CodingSession WHERE Id = @Id)";
            var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = idToUpdate });

            if (exists == 0)
            {
                _userInteraction.ShowMessageTimeout($"[Red]\n\nCoding with ID {id} does not exist.\n\n[/]");
                return;
            }

            // Retrieve the start time
            string startTimeQuery = "SELECT StartTime FROM CodingSession WHERE Id = @Id";
            var startTime = connection.ExecuteScalar<DateTime>(startTimeQuery, new { Id = idToUpdate });

            if (_utils.ValidatedEndTime(startTime, endTime))
                return;

            // Calculate the new duration
            var newDuration = _utils.GetSessionDuration(startTime, endTime);

            // Update the EndTime and Duration
            const string updateQuery = @"
                UPDATE CodingSession
                SET EndTime = @EndTime, Duration = @Duration
                WHERE Id = @Id";

            connection.Execute(updateQuery, new { Id = idToUpdate, EndTime = endTime, Duration = newDuration });
            _userInteraction.ShowMessageTimeout($"[Green]\n\nHabit with ID {id} has been updated.\n\n[/]");
        }

        public void UpdateStartTimeInDatabase(int id, DateTime startTime)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            string checkQuery = "SELECT EXISTS(SELECT 1 From CodingSession WHERE Id = @Id)";
            var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id });

            if (exists == 0)
            {
                _userInteraction.ShowMessageTimeout($"[Red]\n\nHabit with ID {id} does not exist.\n\n[/]");
                return;
            }

            // Retrieve the end time
            string endTimeQuery = "SELECT EndTime FROM CodingSession WHERE Id = @Id";
            var endTime = connection.ExecuteScalar<DateTime>(endTimeQuery, new { Id = id });

            // Calculate the new duration
            var newDuration = _utils.GetSessionDuration(startTime, endTime);

            // Update the StartTime and Duration
            const string updateQuery = @"
                UPDATE CodingSession
                SET StartTime = @StartTime, Duration = @Duration
                WHERE Id = @Id";

            connection.Execute(updateQuery, new { Id = id, StartTime = startTime, Duration = newDuration});
            _userInteraction.ShowMessageTimeout($"[Green]\n\nHabit with ID {id} has been updated.\n\n[/]");
        }
        
        public void GetFromDatabaseOrdered(IEnumerable<CodingSession> list)
        {

            // Display the sessions using Spectre.Console
            if (list.Any())
            {
                var table = new Table();
                table.AddColumn(new TableColumn("Id").RightAligned());
                table.AddColumn("StartTime");
                table.AddColumn("EndTime");
                table.AddColumn("Duration");

                // Count values in IEnumerable<CodingSession> list

                var count = 1;
                foreach (var item in list)
                {
                    table.AddRow(
                        count.ToString(),
                        item.StartTime.ToString("dd-MM-yyyy HH:mm:ss"),
                        item.EndTime.ToString("dd-MM-yyyy HH:mm:ss"),
                        item.Duration
                    );
                    count++;
                }
                _userInteraction.ShowMessage(table);
            }
            else
            {
                _userInteraction.ShowMessageTimeout("\n\n[Red]There are no coding sessions stored in the database.[/]");
            }
        }
    }
}