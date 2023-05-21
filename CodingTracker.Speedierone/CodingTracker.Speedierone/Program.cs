using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodeTracker;

class Program
{
    static void Main ()
    {       
        var connectionString = ConfigurationManager.AppSettings.Get("dbconnectionString");
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS code_tracker(
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            TimeStart TEXT,
            TimeEnd TEXT,
            TimeSpan TEXT)";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
        Console.Clear();
        MainMenu.ShowMenu();
    }
}
