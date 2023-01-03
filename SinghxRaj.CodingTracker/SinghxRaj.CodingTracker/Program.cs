using SinghxRaj.CodingTracker;
using System.Configuration;

internal class Program
{
    static string connectionString = ConfigurationManager.AppSettings.Get("connectionString")!;
    private static void Main(string[] args)
    {
        DatabaseManager.CreateTable(connectionString);

        UserInput.MainMenu();
    }
}