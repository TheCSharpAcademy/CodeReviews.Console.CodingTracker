using CodingTracker.Models;
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
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            StartTime TEXT,
            EndTime TEXT,
            Duration TEXT
        )";

        connection.Execute(sql);
    }

    internal static void Insert(string st, string et, string duration)
    {
        var sql = @"INSERT INTO coding_session (StartTime, EndTime, Duration)
            VALUES (@start, @end, @duration)
        ";

        connection.Execute(sql, new { start = st, end = et, duration });
    }

    internal static List<CodingSession> GetAll()
    {
        var sql = "SELECT * FROM coding_session";
        List<CodingSession> records = connection.Query<CodingSession>(sql).ToList();

        return records;
    }

    internal static void Update(int id, string st, string et, string duration)
    {
        var sql = "UPDATE coding_session SET StartTime = @st, EndTime = @et, Duration = @duration WHERE Id = @id";
        var obj = new { id, st, et, duration };

        connection.Execute(sql, obj);
    }

    internal static bool FindOneSession(int id)
    {
        var sql = "SELECT * FROM coding_session WHERE Id = @id";
        var obj = new { id };

        try
        {
            connection.QuerySingle<CodingSession>(sql, obj);
            return true;
        }
        catch
        {
            return false;
        }
    }
}