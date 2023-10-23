using System.Configuration;

namespace StanChoi.CodingTracker
{
    class Program
    {
        static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
        static void Main(string[] args)
        {
            DatabaseManager databaseManager = new();
            UserInput userInput = new();

            databaseManager.CreateTable(connectionString);
            userInput.MainMenu();
        }
    }
}
