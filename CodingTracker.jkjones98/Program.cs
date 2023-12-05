using System.Configuration;

namespace CodingTracker.jkjones98;

class Program
{
    static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
    static void Main(string[] args)
    {
        DbTableCreator dbTableCreator = new();
        GoalTableCreator goalTableCreator = new();
        MainMenu mainMenu = new();

        dbTableCreator.CreateTable(connectionString);
        goalTableCreator.CreateTable(connectionString);
        mainMenu.DisplayMenu();
    }
}
