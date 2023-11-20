using CodingTracker;
using Microsoft.Data.Sqlite;
using System.Configuration;


//initialize DB
string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();
    var tableCmd = connection.CreateCommand();

    tableCmd.CommandText =
        @"CREATE TABLE IF NOT EXISTS coding_tracker (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            StartTime TEXT,
            EndTime TEXT,
            Duration INTEGER)";

    tableCmd.ExecuteNonQuery();

    connection.Close();
}
TableVisualisationEngine.MainMenu();
