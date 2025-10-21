using System.Configuration;
using System.Data;
using Microsoft.Data.Sqlite;

namespace CodingTracker;

public class Database
{
    public static void Migrate()
    {
        var sAll = ConfigurationManager.AppSettings;
        var connectionString = sAll.Get("ConnectionString");
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using SqliteCommand tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
        @"CREATE TABLE IF NOT EXISTS coding_sessions (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        start_time TEXT,
        end_time TEXT,
        duration INTEGER
        )";

        _ = tableCmd.ExecuteNonQuery();

        connection.Close();

    }
}
