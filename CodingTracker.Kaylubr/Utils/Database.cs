using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.Utils;

internal static class Database
{
    readonly static string? connectionString = Config.InitializeConfig();
    readonly static SqliteConnection connection = new(connectionString);

    internal static void CreateDatabase()
    {
        var sql = @"CREATE TABLE IF NOT EXISTS coding_session (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            start_time TEXT,
            end_time TEXT,
            duration TEXT
        )";

        connection.Execute(sql);
    }

    internal static void Insert(string st, string et, string duration)
    {
        var sql = @"INSERT INTO coding_session (start_time, end_time, duration)
            VALUES (@start, @end, @duration)
        ";

        connection.Execute(sql, new { start = st, end = et, duration });
    }
}