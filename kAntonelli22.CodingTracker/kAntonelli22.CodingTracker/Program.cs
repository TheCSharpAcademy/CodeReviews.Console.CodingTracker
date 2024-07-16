using System.Configuration;
using System.Collections.Specialized;

namespace ConConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            string databasePath = ConfigurationManager.AppSettings.Get("DatabasePath");
            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

            Console.WriteLine($"Database Path: {databasePath}\nConnection String: {connectionString}");

            string query = @"
            CREATE TABLE IF NOT EXISTS Habits (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Habit TEXT NOT NULL,
                Quantity TEXT NOT NULL
            );";


        }
    }
}