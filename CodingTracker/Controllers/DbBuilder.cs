using System.Configuration;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.Controllers;

public static class DbBuilder
{
    private static readonly string ConnectionString =
        ConfigurationManager.ConnectionStrings["DbConnector"].ConnectionString;

    public static void CreateTable()
    {
        if (!File.Exists("coding-tracker.db"))
        {
            using var connection = GetConnection();
            var sql =
                @"CREATE TABLE IF NOT EXISTS coding_tracker(
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                startTime TEXT,
                endTime TEXT,
                duration TEXT
                )";

            connection.Execute(sql);

            CrudManager.SeedDatabase();
        }
    }

    public static SqliteConnection GetConnection()
    {
        return new SqliteConnection(ConnectionString);
    }
}