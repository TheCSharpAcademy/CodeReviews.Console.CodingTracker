namespace CodingTracker;

internal static class Menu
{

    internal static void ShowMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Main Menu\n");
            Console.WriteLine("What would you like to do?\n");
            Console.WriteLine("0 To Close Application.");
            Console.WriteLine("1 To Add Session.");
            Console.WriteLine("2 To Update Session.");
            Console.WriteLine("3 To Delete Session.");
            Console.WriteLine("4 To View All Records.");
            Console.WriteLine("----------------------------------");
            GetMainMenuOption();
        }
    }

    private static void GetMainMenuOption()
    {
        switch (Console.ReadLine()?.Trim())
        {
            case "0":
                Environment.Exit(0);
                break;
            case "1":
                DatabaseManager.AddSession();
                break;
            case "2":
                DatabaseManager.UpdateSession();
                break;
            case "3":
                DatabaseManager.DeleteSession();
                break;
            case "4":
                DatabaseManager.GetSessions();
                Console.WriteLine("\n\nPress any key to return to main menu....");
                Console.ReadKey();
                break;
            default:
                Console.WriteLine("Please Enter a valid numeric value.");
                GetMainMenuOption();
                break;
        }
    }
}
