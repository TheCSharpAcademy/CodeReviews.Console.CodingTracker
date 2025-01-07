using Dapper;
using Microsoft.Data.Sqlite;

internal class Database
{
    public static void Initialize()
    {
        using var connection = new SqliteConnection(Config.ConnectionString);
        connection.Open();
        connection.Execute(@"
        CREATE TABLE IF NOT EXISTS sessions (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            start_time DATETIME NOT NULL,
            end_time DATETIME NOT NULL,
            duration INTEGER NOT NULL DEFAULT 0);

        CREATE TABLE IF NOT EXISTS goals (
            id INTEGER PRIMARY KEY,
            total_hours INTEGER NOT NULL DEFAULT 0,
            average_hours INTEGER NOT NULL DEFAULT 0,
            set_goal_date DATE NOT NULL DEFAULT '1000-01-01');");

        var hasRows = connection.QueryFirstOrDefault<int>("SELECT 1 FROM goals") != 0;
        if (!hasRows)
        {
            var goal = new Goal
            {
                TotalHours = 0,
                AverageHours = 0,
                SetGoalDate = DateTime.Parse("1000-01-01")
            };
            connection.Execute(@"
                INSERT INTO goals (total_hours, average_hours, set_goal_date)
                VALUES (@TotalHours, @AverageHours, @SetGoalDate)",
                goal);
        }
    }
}
