using Dapper;
using Microsoft.Data.Sqlite;

namespace coding_tracker;

internal class DatabaseManager
{
    public void SqlInitialize(string connectionString)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sqlMain =
                @"CREATE TABLE IF NOT EXISTS Coding_Session (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                StartTime TEXT,
                EndTime TEXT,
                Duration TEXT)";
            string sqlGoal =
                @"CREATE TABLE IF NOT EXISTS Coding_Goal (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Hours INTEGER,
                Completed INTEGER,
                Datestamp TEXT)";
            string sqlGoalScore =
                @"CREATE TABLE IF NOT EXISTS All_Coding_Goals (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Hours INTEGER,
                Completed INTEGER)";
            connection.Execute(sqlMain);
            connection.Execute(sqlGoal);
            connection.Execute(sqlGoalScore);
        }
    }
}
