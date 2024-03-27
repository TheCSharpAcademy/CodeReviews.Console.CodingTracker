using System.Data.SQLite;
using Dapper;

namespace CodingTracker;

public class Database
{
    public static void Initialize(string connectionString)
    {
        string sql =
        @"
            CREATE TABLE IF NOT EXISTS CodingTracker (
                Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT NOT NULL,
                EndTime TEXT NOT NULL,
                Duration TEXT NOT NULL
            );
        ";

        using var connection = new SQLiteConnection(connectionString);
        connection.Execute(sql);
    }
}