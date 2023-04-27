using Microsoft.Data.Sqlite;

namespace CodingTracker.jwhitt3r
{
    /// <summary>
    /// Ensures that a database table is correctly setup before any interaction has occurred
    /// </summary>
    internal class DatabaseManager
    {
        internal void CreateTable(string connectionString)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText =
                        @"CREATE TABLE IF NOT EXISTS coding (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Date TEXT,
                            Duration TEXT,
                            StartTime TEXT,
                            EndTime TEXT
                        )";

                    tableCmd.ExecuteNonQuery();
                }
            }
        }
    }
}