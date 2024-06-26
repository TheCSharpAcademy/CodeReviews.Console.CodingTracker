using System.Data;
using Microsoft.Data.Sqlite;
using Dapper;
using jollejonas.CodingTracker.Models;

namespace jollejonas.CodingTracker.Data
{
    public static class DatabaseManager
    {
        public static IDbConnection Connection(string connectionString)
        {
            var connection = new SqliteConnection(connectionString);
            connection.Open();
            return connection;
        }

        public static void EnsureSessionDatabaseCreated(IDbConnection db)
        {
            string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS CodingSessions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT NOT NULL,
                EndTime TEXT NOT NULL,
                Duration REAL NOT NULL
            )";

            db.Execute(createTableQuery);
        }
        public static void EnsureGoalDatabaseCreated(IDbConnection db)
        {
            string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS Goals (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Duration REAL NOT NULL
            )";

            db.Execute(createTableQuery);
        }

        
    }
}
