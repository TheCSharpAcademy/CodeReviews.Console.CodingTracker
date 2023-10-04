using System.Configuration;
using CodingTracker.Services;

namespace CodingTracker;

public class CodingTracker
{
    private static readonly string dbFile = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
    public static void Main(string[] args)
    {
        if (!File.Exists(dbFile))
        {
            Database.CreateDatabase();
            MainMenu.ShowMenu();
        }
        else
        {
            MainMenu.ShowMenu();
        }
    }
}