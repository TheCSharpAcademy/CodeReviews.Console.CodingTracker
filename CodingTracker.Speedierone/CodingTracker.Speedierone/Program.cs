using Microsoft.Data.Sqlite;

namespace CodeTracker;

//TODO come back to link to config file.
class Program
{
    static string connectionString = "Data Source=Coding-Tracker.db";
    static void Main(string[] args)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS code_tracker(
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            TimeSpan TEXT)";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
        Console.Clear();
        MainMenu.ShowMenu();
    }
}
