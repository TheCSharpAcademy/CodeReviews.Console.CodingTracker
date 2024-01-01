
using codingTracker.Ibrahim.data;
using codingTracker.Ibrahim.Helpers;
using codingTracker.Ibrahim.UI;
namespace codingTracker.Ibrahim.UI
{
    public class UserMenu
    {
        public static void showMenu()
        {
            bool endApp = false;

            while (!endApp)
            {
                Console.WriteLine("Coding Time Tracker App \n");
                Console.WriteLine("Main Menu: \n");
                Console.WriteLine("Press 0 to exit");
                Console.WriteLine("Press 1 to add a coding session");
                Console.WriteLine("Press 2 to update a coding session");
                Console.WriteLine("Press 3 to delete a coding session");
                Console.WriteLine("Press 4 to view all your coding sessions");
                Console.WriteLine("Press 5 to view your analytics \n");
                Console.Write($"Enter here: ");
                string userChoice = helper.ValidateUserChoice(Console.ReadLine());
                switch (userChoice)
                {
                    case "0":
                        Environment.Exit(0);
                        break;
                    case "1":
                        Console.Write("Enter the Date and Time you started your coding session in the format 'MM-dd-yyyy HH:mm:ss'");
                        string StartTime= Console.ReadLine();
                        Console.Write("Enter the Date and Time you ended your coding session in the format 'MM-dd-yyyy HH:mm:ss'");
                        string EndTime= Console.ReadLine();
                        (StartTime, EndTime)= helper.ValidateDateTimes(StartTime, EndTime);
                        DatabaseManager.InsertData(StartTime, EndTime);
                        break;
                    case "2":
                       // DatabaseManager.UpdateData(Id, StartTime, EndTime);
                        break;
                    case "3":
                       // DatabaseManager.DeleteData(Id);
                        break;
                    case "4":
                        TableVisualizationEngine.ShowTable(DatabaseManager.GetALLData());
                        break;
                    case "5":
                        DatabaseManager.GetReports();
                        break;
                    default:
                        Console.WriteLine("that option isn't available yet press any key to go back to the main menu");
                        break;

                }
            }
        }
    }
}
