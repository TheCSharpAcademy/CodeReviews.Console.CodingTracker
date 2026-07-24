using CodingTracker.CSharpAcademy_Learner.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.CSharpAcademy_Learner.Controllers
{
    internal class CodingController
    {
        internal List<CodingSession> GetAllSessions()
        {
            using (var connection = new SqliteConnection(Configuration.GetConnectionString()))
            {
                var querySQL = "SELECT * FROM coding_sessions";
                var sessions = connection.Query<CodingSession>(querySQL).ToList();

                return sessions;
            }
        }

        internal void InsertSession(string startTime, string endTime, int duration)
        {
            using (var connection = new SqliteConnection(Configuration.GetConnectionString()))
            {
                var parameters = new { StartTime = startTime, EndTime = endTime, Duration = duration };
                var insertSQL = "INSERT INTO coding_sessions(StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)";
                connection.Execute(insertSQL, parameters);
            }
        }

        internal void UpdateSession(int sessionId, string startTime, string endTime, int duration)
        {
            using (var connection = new SqliteConnection(Configuration.GetConnectionString()))
            {
                var parameters = new { Id = sessionId, StartTime = startTime, EndTime = endTime, Duration = duration };
                var updateSQL = "UPDATE coding_sessions SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration WHERE Id = @Id";
                connection.Execute(updateSQL, parameters);
            }
        }

        internal void DeleteSession(int sessionId)
        {
            using (var connection = new SqliteConnection(Configuration.GetConnectionString()))
            {
                var parameters = new { Id = sessionId };
                var deleteSQL = "DELETE FROM coding_sessions WHERE Id = @Id";
                connection.Execute(deleteSQL, parameters);
            }
        }
    }
}
