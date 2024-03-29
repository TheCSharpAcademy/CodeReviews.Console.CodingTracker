using Microsoft.Data.Sqlite;
using Dapper;

namespace coding_tracker;

internal class DatabaseManager
{
    public void SqlInitialize(string connectionString)
    {
        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery =
                @"CREATE TABLE IF NOT EXISTS Coding_Session (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT,
                EndTime TEXT,
                Duration TEXT)";
            connection.Execute(sqlQuery);
        }
    }
}
