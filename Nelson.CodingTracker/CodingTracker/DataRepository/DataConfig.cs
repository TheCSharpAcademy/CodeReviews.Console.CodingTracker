using System.Configuration;
using CodingTracker.IDataRepository;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.DataRepository
{
    public class DataConfig : IDataConfig
    {
        readonly string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString") ?? "";

        public void InitializeDatabase()
        {
            // Check if the database is already initialized
            // If not, create the database and tables

            // Initialize the database
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            const string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS CodingSession (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    StartTime TEXT,
                    EndTime TEXT,
                    Duration TEXT
                );";

            connection.Execute(createTableQuery);
        }
    }
}