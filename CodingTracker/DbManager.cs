using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker;

public static class DbManager
{
    private static string dbConnector = ConfigurationManager.ConnectionStrings["DbConnector"].ConnectionString;
    
    public static void CreateTable()
    {
        using (var connection = new SqliteConnection(dbConnector))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS coding_tracker(
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                start_time TEXT,
                end_time TEXT,
                duration TEXT
                )";

            tableCmd.ExecuteNonQuery();
        }
    }
}