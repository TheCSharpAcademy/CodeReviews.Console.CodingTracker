using System.Configuration;

namespace CodingTracker
{
    class Program
    {
        static string? connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        public static void Main(string[] args)
        {
            DatabaseManager databaseManager = new();
            GetUserInput getUserInput = new();
            databaseManager.CreateTable(connectionString);
            getUserInput.MainMenu();
        }
    }
}


