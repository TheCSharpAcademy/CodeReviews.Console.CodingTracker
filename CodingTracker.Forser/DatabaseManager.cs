using Microsoft.Data.Sqlite;

internal class DatabaseManager
{
    static string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString");

    internal void CreateTable()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS Coding (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    StartDate TEXT,
                    EndDate TEXT,
                    TotalDuration REAL
                )";

            tableCmd.ExecuteNonQuery();
        }
    }
}