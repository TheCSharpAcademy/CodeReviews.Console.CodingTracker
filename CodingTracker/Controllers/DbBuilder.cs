using System.Configuration;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.Controllers;

public static class DbBuilder
{
    private static string connectionString = ConfigurationManager.ConnectionStrings["DbConnector"].ConnectionString;
    
    public static void CreateTable()
    {
        using (var connection = GetConnection())
        {
            string sql =
                @"CREATE TABLE IF NOT EXISTS coding_tracker(
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                startTime TEXT,
                endTime TEXT,
                duration TEXT
                )";

            connection.Execute(sql);
        }
    }

    public static SqliteConnection GetConnection()
    {
        return new SqliteConnection(connectionString);
    }
}
