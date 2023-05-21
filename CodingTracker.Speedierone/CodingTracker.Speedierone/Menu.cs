namespace CodeTracker;

internal class MainMenu
{
    internal static void ShowMenu()
    {
        Console.Clear();
        bool closeApp = false;
        while (closeApp == false)
        {
            Console.Clear() ;
            Console.WriteLine("Welcome, what would you like to do today?");
            Console.WriteLine("Type 0 to exit program");
            Console.WriteLine("Type 1 to view records");
            Console.WriteLine("Type 2 to add new session");
            Console.WriteLine("Type 3 to delete session");
            Console.WriteLine("Type 4 to update session");
            Console.WriteLine("Type 5 to add timed session");

            var command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("Goodbye");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    UserInput.ViewRecords();
                    break;
                case "2":
                    UserInput.AddRecord();
                    break;
                case "3":
                    UserInput.DeleteRecord();
                    break;
                case "4":
                    UserInput.UpdateRecord();
                    break;
                case "5":
                    UserInput.AddTimerRecord();
                    break;
                default:
                    Console.WriteLine("Invalid Input. Press enter to try again.");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                    
            }
                
        }
    }
}
