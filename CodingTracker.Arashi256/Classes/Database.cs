using CodingTracker.Arashi256.Config;
using CodingTracker.Arashi256.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.Arashi256.Classes
{
    internal class Database
    {
        private DatabaseConnection _connection;

        public Database()
        {
            _connection = new DatabaseConnection();
            if (CheckDatabase(false))
                Console.WriteLine("Database reset and ready.");
            else
                Console.WriteLine("Database already present and correct.");
        }

        private void ExecuteQuery(string query, DynamicParameters? parameters = null)
        {
            using (var connection = new SqliteConnection(_connection.DatabaseConnectionString))
            {
                connection.Execute(query, parameters);
            }
        }

        public int TableExists(string tableName)
        {
            string sql = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = @TableName";
            int result = 0;
            using (var connection = new SqliteConnection(_connection.DatabaseConnectionString))
            {
                result = connection.QueryFirstOrDefault<int>(sql, new { TableName = tableName });
            }
            return result;
        }

        public List<CodingSession> GetCodingSessionResults(string query, DynamicParameters? parameters = null)
        {
            List<CodingSession> codingSessions = new List<CodingSession>();
            using (var connection = new SqliteConnection(_connection.DatabaseConnectionString))
            {
                codingSessions = connection.Query<CodingSession>(query, parameters).AsList();
            }
            return codingSessions;
        }

        public int AddNewCodingSession(CodingSession codingSession)
        {
            using (var connection = new SqliteConnection(_connection.DatabaseConnectionString))
            {
                var sql = "INSERT INTO coding_session (StartDateTime, EndDateTime, Duration) VALUES (@StartDateTime, @EndDateTime, @Duration)";
                var parameters = new DynamicParameters();
                parameters.Add("@StartDateTime", codingSession.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                parameters.Add("@EndDateTime", codingSession.EndDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                parameters.Add("@Duration", codingSession.Duration);
                var rowsAffected = connection.Execute(sql, parameters);
                return rowsAffected;    
            }
        }

        public int UpdateCodingSession(CodingSession codingSession)
        {
            using (var connection = new SqliteConnection(_connection.DatabaseConnectionString))
            {
                string sql = "UPDATE coding_session SET StartDateTime = @StartDateTime, EndDateTime = @EndDateTime, Duration = @Duration WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", codingSession.Id);
                parameters.Add("@StartDateTime", codingSession.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                parameters.Add("@EndDateTime", codingSession.EndDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                parameters.Add("@Duration", codingSession.Duration);
                var rowsAffected = connection.Execute(sql, parameters);
                return rowsAffected;
            }
        }

        public int DeleteCodingSession(CodingSession codingSession)
        {
            using (var connection = new SqliteConnection(_connection.DatabaseConnectionString))
            {
                string sql = "DELETE FROM coding_session WHERE Id = @Id";
                var rowsAffected = connection.Execute(sql, codingSession);
                return rowsAffected;
            }
        }

        public bool CheckDatabase(bool force)
        {
            int creationCount = 0;
            creationCount += TableExists("coding_session");
            creationCount += TableExists("coding_goal");
            if (creationCount < 2 || force)
            {
                ExecuteQuery("DROP TABLE IF EXISTS coding_session");
                ExecuteQuery("CREATE TABLE IF NOT EXISTS coding_session (Id INTEGER PRIMARY KEY AUTOINCREMENT, StartDateTime TEXT, EndDateTime TEXT, Duration TEXT)");
                ExecuteQuery("DROP TABLE IF EXISTS coding_goal");
                ExecuteQuery("CREATE TABLE IF NOT EXISTS coding_goal (Id INTEGER PRIMARY KEY AUTOINCREMENT, StartDateTime TEXT, Hours INTEGER, DeadlineDateTime TEXT)");
                return true;
            }
            return false;
        }

        public List<CodingGoal> GetCodingGoalResults(string query, DynamicParameters? parameters = null)
        {
            List<CodingGoal> codingGoals = new List<CodingGoal>();
            if (!query.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("ERROR: Query must start with SELECT.", nameof(query));
            }
            using (var connection = new SqliteConnection(_connection.DatabaseConnectionString))
            {
                codingGoals = connection.Query<CodingGoal>(query, parameters).AsList();
            }
            return codingGoals;
        }

        public int AddNewCodingGoal(CodingGoal codingGoal)
        {
            using (var connection = new SqliteConnection(_connection.DatabaseConnectionString))
            {
                var sql = "INSERT INTO coding_goal(StartDateTime, Hours, DeadlineDateTime) VALUES (@StartDateTime, @Hours, @DeadlineDateTime)";
                var parameters = new DynamicParameters();
                parameters.Add("@StartDateTime", codingGoal.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                parameters.Add("@Hours", codingGoal.Hours);
                parameters.Add("@DeadlineDateTime", codingGoal.DeadlineDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                var rowsAffected = connection.Execute(sql, parameters);
                return rowsAffected;
            }
        }

        public int UpdateCodingGoal(CodingGoal codingGoal)
        {
            using (var connection = new SqliteConnection(_connection.DatabaseConnectionString))
            {
                string sql = "UPDATE coding_goal SET Hours = @Hours, DeadlineDateTime = @DeadlineDateTime WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", codingGoal.Id);
                parameters.Add("@Hours", codingGoal.Hours);
                parameters.Add("@DeadlineDateTime", codingGoal.DeadlineDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                var rowsAffected = connection.Execute(sql, parameters);
                return rowsAffected;
            }
        }

        public int DeleteCodingGoal()
        {
            using (var connection = new SqliteConnection(_connection.DatabaseConnectionString))
            {
                string sql = "DELETE FROM coding_goal";
                var rowsAffected = connection.Execute(sql);
                return rowsAffected;
            }
        }
    }
}
