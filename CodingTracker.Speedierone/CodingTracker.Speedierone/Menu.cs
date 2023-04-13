namespace CodingTracker;

internal class MainMenu
{
    internal static void ShowMenu()
    {
        Console.Clear();
        bool closeapp = false;
        while (closeapp == false)
        {
            Console.WriteLine("Welcome, what would you like to do today?");
            Console.WriteLine("Type 0 to exit program");
            Console.WriteLine("Type 1 to view records");
            Console.WriteLine("Type 2 to add new session");
            Console.WriteLine("Type 3 to delete session");
            Console.WriteLine("Type 4 to update session");

            var command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("Goodbye");
                    closeapp = true;
                    Environment.Exit(0);
                    break;
                case "1":
 //                   ViewRecords();
 //                   break;
                case "2":
                       UserInput.AddRecord();
                       break;
                case "3":
 /*                   DeleteRecord();
                        break;
                case "4":
                    UpdateRecord();
                    break;*/
                default:
                    Console.WriteLine("Invalid Input");
                    Console.Clear();
                    break;
                    
            }
                
        }
    }
}
