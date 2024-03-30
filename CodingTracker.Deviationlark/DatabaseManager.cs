using Microsoft.Data.Sqlite;

namespace CodingTracker
{
    internal class DatabaseManager
    {
        internal void CreateTable(string? connectionString)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS coding_hours(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Duration TEXT
                )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS coding_goals(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Hours TEXT,
                    Date TEXT,
                    RemainingDays TEXT,
                    RemainingHours TEXT,
                    HoursPerDay TEXT
                )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}