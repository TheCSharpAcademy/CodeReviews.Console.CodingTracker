using System.Configuration;

namespace CodingTracker.jkjones98;

class Program
{
    static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
    static void Main(string[] args)
    {
        DbTableCreator dbTableCreator = new();
        MainMenu mainMenu = new();

        dbTableCreator.CreateTable(connectionString);
        mainMenu.DisplayMenu();
    }
}
