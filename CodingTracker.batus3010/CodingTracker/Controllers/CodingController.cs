using CodingTracker.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.Controllers
{
    public class CodingController
    {
        private readonly string _connectionString;

        public CodingController(string connectionString)
        {
            _connectionString = connectionString;
            InitialzieDatabase();
        }
        
        private void InitialzieDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS CodingSessions (
                                            Id INTEGER PRIMARY KEY,
                                            StartTime TEXT,
                                            EndTime TEXT
                                        )";
            tableCmd.ExecuteNonQuery();

            connection.Close();
        }

        public void AddSession(CodingSession session)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var sql = "INSERT INTO CodingSessions (StartTime, EndTime) VALUES (@StartTime, @EndTime)";
            connection.Execute(sql, new { session.StartTime, session.EndTime });
            connection.Close();
        }

        public List<CodingSession> GetSessions()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var sql = "SELECT * FROM CodingSessions";
            var sessions = connection.Query<CodingSession>(sql).ToList();
            connection.Close();
            return sessions;
        }

        public void DeleteSession(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var sql = "DELETE FROM CodingSessions WHERE Id = @Id";
            connection.Execute(sql, new { Id = id });
            connection.Close();
        }

        public void UpdateSession(CodingSession session)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var sql = "UPDATE CodingSessions SET StartTime = @StartTime, EndTime = @EndTime WHERE Id = @Id";
            connection.Execute(sql, new { session.Id, session.StartTime, session.EndTime });
            connection.Close();
        }

    }
}