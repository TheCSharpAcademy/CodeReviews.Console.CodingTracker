
using Microsoft.Data.Sqlite;

internal class DatabaseManager
{
    internal static void CreateTable(string connectionString)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS coding_sessions (
                Id INTEGER PRIMARY KEY,
                StartDateTime TEXT,
                EndDateTime TEXT,
                Duration TEXT)";

            tableCmd.ExecuteNonQuery();
        }
    }
}