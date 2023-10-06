using CodingTracker.Services;

namespace CodingTracker;

public class CodingTracker
{
    private static readonly string dbFile = "coding_logs.db";
    public static void Main(string[] args)
    {
        Console.WriteLine($"Welcome To Your Coding Tracker!");
        Console.WriteLine($"Current Date: {Helpers.GetDate(false)}");
        Thread.Sleep(2000);

        if (!File.Exists(dbFile))
        {
            Database.CreateDatabase();
            Console.Clear();
            MainMenu.ShowMenu();
        }
        else
        {
            Console.Clear();
            MainMenu.ShowMenu();
        }
    }
}