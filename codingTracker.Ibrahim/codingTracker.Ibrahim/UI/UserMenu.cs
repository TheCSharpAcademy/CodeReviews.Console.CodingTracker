
using System.Diagnostics;
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
                Console.WriteLine("Press 5 to start a coding session timer \n");
                Console.Write($"Enter here: ");
                string userChoice = helper.ValidateUserChoice(Console.ReadLine());
                switch (userChoice)
                {
                    case "0":
                        Environment.Exit(0);
                        break;
                    case "1":
                        Console.Clear();
                        Console.WriteLine("Enter the Date and Time you started your coding session in the format MM-DD-YYYY HH:MM AM/PM (e.g., 04-26-2001 1:00 PM)");
                        Console.Write("\nEnter here: ");
                        string StartTime = Console.ReadLine();
                        Console.WriteLine("\nEnter the Date and Time you ended your coding session in the format MM-DD-YYYY HH:MM AM/PM (e.g., 04-26-2001 1:00 PM)");
                        Console.Write("\nEnter here: ");
                        string EndTime = Console.ReadLine();
                        (StartTime, EndTime) = helper.ValidateDateTimes(StartTime, EndTime);
                        DatabaseManager.InsertData(StartTime, EndTime);
                        break;
                    case "2":
                        Console.Clear();
                        Console.Write("please enter the coding session Id you wish to update: ");
                        int Id = helper.validateInt(Console.ReadLine());
                        Console.Clear();
                        Console.WriteLine("Enter the Date and Time you started your coding session in the format MM-DD-YYYY HH:MM AM/PM (e.g., 04-26-2001 1:00 PM)");
                        Console.Write("\nEnter here: ");
                        StartTime = Console.ReadLine();
                        Console.WriteLine("\nEnter the Date and Time you ended your coding session in the format MM-DD-YYYY HH:MM AM/PM (e.g., 04-26-2001 1:00 PM)");
                        Console.Write("\nEnter here: ");
                        EndTime = Console.ReadLine();
                        (StartTime, EndTime) = helper.ValidateDateTime(Id, StartTime, EndTime);
                        DatabaseManager.UpdateData(Id, StartTime, EndTime);
                        break;
                    case "3":
                        Console.Clear();
                        Console.Write("please enter the coding session Id you wish to delete: ");
                        Id = helper.validateInt(Console.ReadLine());
                        DatabaseManager.DeleteData(Id);
                        break;
                    case "4":
                        Console.Clear();
                        TableVisualizationEngine.ShowTable(DatabaseManager.GetALLData());
                        Console.WriteLine("press any key to go back");
                        Console.ReadLine();
                        break;
                    
                    case "5":
                        Console.Clear();
                        Console.WriteLine("Press any key to start timer");
                        Console.ReadLine();
                        SessionTracker session = new SessionTracker();
                        session.StartTimer();
                        Console.Clear();
                        Console.WriteLine("Stopwatch started (press any key to stop)... \n");
                        while (!Console.KeyAvailable)
                        {
                            Console.SetCursorPosition(0, Console.CursorTop);
                            Console.Write($"Elapsed time: {session.stopwatch.Elapsed:hh\\:mm\\:ss}");
                            Thread.Sleep(100);
                        }
                        session.EndTimer();
                        var time = session.GetTime();
                        DatabaseManager.InsertData(time.startTime,time.endTime);
                        Console.WriteLine("\npress any key to go back");
                        Console.ReadKey();
                        Console.ReadLine();  
                        break;
                    default:
                        Console.WriteLine("that option isn't available yet press any key to go back to the main menu");
                        break;

                }
               
                Console.Clear();
            }
        }
    }
}
