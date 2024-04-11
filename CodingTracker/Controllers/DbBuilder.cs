using System.Configuration;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.Controllers;

public static class DbBuilder
{
    private static string connectionString = ConfigurationManager.ConnectionStrings["DbConnector"].ConnectionString;
    
    public static void CreateTable()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            string sql =
                @"CREATE TABLE IF NOT EXISTS coding_tracker(
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                start_time TEXT,
                end_time TEXT,
                duration TEXT
                )";

            connection.Execute(sql);
        }
    }
}
