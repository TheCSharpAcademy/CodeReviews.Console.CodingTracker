using System.Data.Common;
using System.Configuration;

namespace CodingTracker.SamGannon
{
    internal class Program
    {
        
        static string connectionString = ConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString;

        static void Main(string[] args)
        {
            DatabaseManager databaseManager = new();
            GetUserInput getUserInput = new();

            databaseManager.CreateCodingTable(connectionString);
            databaseManager.CreateSleepTable(connectionString);
            getUserInput.MainMenu();
        }
    }
}