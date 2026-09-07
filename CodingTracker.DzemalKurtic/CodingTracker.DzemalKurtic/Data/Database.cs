using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.DzemalKurtic.Data;

internal static class Database
{
    public static void Initialize(string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var sql = 
            """
            CREATE TABLE IF NOT EXISTS coding_sessions (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            StartTime TEXT NOT NULL,
            EndTime TEXT NOT NULL
            );
            """;

        connection.Execute(sql);
    }
}
