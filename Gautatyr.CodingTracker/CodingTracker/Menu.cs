using static CodingTracker.DataAccess;
using static CodingTracker.Helpers;

namespace CodingTracker;

public static class Menu
{
    public static void DisplayMenu(string message = "")
    {
        Console.Clear();
        Console.WriteLine("\nMAIN MENU\n");
        Console.WriteLine("- Type 0 to Close the Application.");
        Console.WriteLine("- Type 1 to View your coding sessions history.");
        Console.WriteLine("- Type 2 to Add a coding session.");
        Console.WriteLine("- Type 3 to Delete a session history.");
        Console.WriteLine("- Type 4 to Update a session history.");
        Console.WriteLine(message);

        string input = Console.ReadLine();

        switch (input)
        {
            case "0":
                Console.WriteLine("\nGoodbye !");
                Environment.Exit(0);
                break;
            case "1":
                Console.Clear();
                DisplaySessions(GetSessionsHistory(), "\nPress Enter to go back to the menu");
                Console.ReadLine();
                break;
            case "2":
                string check;
                do
                {
                    DisplaySessions(GetSessionsHistory());
                    check = InsertSession();
                    if (check == "1")
                    {
                        DisplaySessions(GetSessionsHistory(), "\nSession successfully added ! ");
                    }
                } while (check != "0" && AskToContinueOperation() == 1);

                break;
            case "3":
                int idDelete = 0;

                do
                {
                    DisplaySessions(GetSessionsHistory(), "\nType in the id of the session you want to Delete, or type 0 to go back to the main menu\n", true);
                    idDelete = GetNumberInput();
                    while (DeleteSession(idDelete) == 0 && idDelete != 0)
                    {
                        Console.WriteLine($"\n|---> Record with Id {idDelete} doesn't exist. Please retry. <---|\n");
                        idDelete = GetNumberInput("\nType in the id of the session you want to Delete, or 0 to get back to the main menu\n");
                    }
                    if (idDelete != 0)
                    {
                        DisplaySessions(GetSessionsHistory(), $"\nThe session with id:{idDelete} has been deleted !\n");
                    }
                } while (idDelete != 0 && AskToContinueOperation() == 1);
                break;
            case "4":
                int menu;

                do
                {
                    DisplaySessions(GetSessionsHistory(), "\nType in the id of the session you want to Update, or 0 to get back to the main menu\n", true);
                    int idUpdate = GetNumberInput();

                    while ((menu = UpdateSession(idUpdate)) == 0 && idUpdate != 0)
                    {
                        Console.WriteLine($"\n|---> Record with Id {idUpdate} doesn't exist. Please retry. <---|\n");
                        idUpdate = GetNumberInput("\nType in the id of the session you want to Update, or 0 to get back to the main menu\n");
                    }

                    if (idUpdate != 0 && menu != 2)
                    {
                        DisplaySessions(GetSessionsHistory(), $"\nThe session with id:{idUpdate} has been updated !\n");
                    }
                } while (menu != 0 && AskToContinueOperation() == 1);

                break;
            default:
                DisplayMenu("\n|---> Invalid Input ! Please type a number from 0 to 4 ! <---|\n");
                break;
        }
    }
}
