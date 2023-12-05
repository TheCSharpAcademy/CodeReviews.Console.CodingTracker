using Microsoft.Data.Sqlite;

namespace CodingTracker.jkjones98
{
    internal class GoalTableCreator
    {
        internal void CreateTable(string connectionString)
        {
            using(var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using(var tableCmd = connection.CreateCommand())
                {   
                    tableCmd.CommandText = 
                        @"CREATE TABLE IF NOT EXISTS goals (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            GoalName TEXT,
                            GoalHours REAL,
                            HoursDone REAL,
                            HoursLeft REAL
                        )";
                    tableCmd.ExecuteNonQuery();
                }
            }
        }
    }
}