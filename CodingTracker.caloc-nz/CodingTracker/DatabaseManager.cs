using Microsoft.Data.Sqlite;
using Dapper;

namespace CodingTracker;

public class DatabaseManager
{
    internal void CreateTable(string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        {
                connection.Open();
                string sql =
                    @"CREATE TABLE IF NOT EXISTS coding (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Date TEXT,
                            Duration TEXT
                        )";
                connection.Execute(sql);
        }
    }
}