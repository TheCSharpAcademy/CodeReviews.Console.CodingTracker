using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace CodingTracker.SamGannon
{
    internal class DatabaseManager
    {
        public DatabaseManager()
        {

        }

        public void CreateCodingTable(string connectionString)
        {
            using (var connection = new SqliteConnection(connectionString))
            { 
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS coding (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Duration TEXT
                )";
                tableCmd.ExecuteNonQuery();

            }
        }

        public void CreateSleepTable(string connectionString)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS sleep (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Duration TEXT,
                    SleepType TEXT
                )";
                tableCmd.ExecuteNonQuery();

            }
        }
    }
}