using System.Configuration;

namespace CodingTracker.StevieTV;

class CodingTracker
{
    private static readonly string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

    static void Main()
    {
        DatabaseManager databaseManager = new();
        GetUserInput getUserInput = new();
        
        databaseManager.CreateTable(connectionString);
        getUserInput.MainMenu();
    }
}