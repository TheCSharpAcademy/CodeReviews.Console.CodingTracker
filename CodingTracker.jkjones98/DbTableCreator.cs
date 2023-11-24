using Microsoft.Data.Sqlite;

namespace CodingTracker.jkjones98
{
    internal class DbTableCreator
    {
        internal void CreateTable(string connectionString)
        {
            using(var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using(var tableCmd = connection.CreateCommand())
                {   
                    tableCmd.CommandText = 
                        @"CREATE TABLE IF NOT EXISTS coding (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Date TEXT,
                            StartTime TEXT,
                            EndTime TEXT,
                            Duration TEXT
                        )";
                    tableCmd.ExecuteNonQuery();
                }
            }
        }
    }
}