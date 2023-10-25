using Microsoft.Data.Sqlite;

namespace StanChoi.CodingTracker
{
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
                        @"CREATE TABLE IF NOT EXISTS coding_session (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT,
                        EndTime TEXT,
                        Duration TEXT
                    )";

                    tableCmd.ExecuteNonQuery();
                }
            }
        }
    }
}
