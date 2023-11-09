using Microsoft.Data.Sqlite;

namespace CodingTracker.SamGannon
{
    internal class DatabaseManager
    {
        public DatabaseManager()
        {

        }

        public void CreateTable(string connectionString)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();

                    tableCmd.CommandText =
                        @"CREATE TABLE IF NOT EXISTS coding (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Duration TEXT
                    )";
                    tableCmd.ExecuteNonQuery();
                }
              
            }
        }

    }
}