using System.Configuration;

namespace CodingTracker.Ledana
{
    internal class Program
    {
        
        static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
        static void Main(string[] args)
        {
            DatabaseManager databaseManager = new();
            databaseManager.CreateTable(connectionString);
            GetUserInput getUserInput = new();
            getUserInput.MainMenu();
        }
    }
}
