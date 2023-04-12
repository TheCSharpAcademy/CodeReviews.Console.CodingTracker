using System.Configuration;

namespace CodingTracker.jwhitt3r
{
    class Program
    {
        static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
        /// <summary>
        /// Simple entry to the program
        /// </summary>
        static void Main()
        {

            GetUserInput getUserInput = new();
            DatabaseManager databaseManager = new();
            databaseManager.CreateTable(connectionString);

            getUserInput.MainMenu();

        }
    }
}