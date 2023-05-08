using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CodingTracker
{
    public class UserInput
    {
        static string connectionString = System.Configuration.ConfigurationManager.AppSettings["connectionString"];

        public static void GetUserInput()
        {
            bool exit = false;
            Console.WriteLine($"\nWelcome to JsPeanut's CodingTracker! You can start tracking your coding sessions. Here you've got a list of all the available commands in the application: \n\n C: Insert the dates in which you started and finished your session, to calculate it. \n S: Start tracking a coding session via stopwatch. \n R: See all your coding sessions.\n U: Update a coding session.\n D: Delete a coding session.\n E: Exit the application.");
            string userInput = Console.ReadLine();
            while (exit == false)
            {
                switch (userInput)
                {
                    case "C":
                        CodingController.Insert();
                        break;
                    case "S":
                        CodingController.StopwatchSession();
                        break;
                    case "R":
                        Console.Clear();
                        CodingController.GetAllRecords();
                        UserInput.GetUserInput();
                        break;
                    case "U":
                        CodingController.Update();
                        break;
                    case "D":
                        CodingController.Delete();
                        break;
                    case "E":
                        Console.WriteLine("You have exited the app successfully.");
                        exit = true;
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("That option does not exist.");
                        GetUserInput();
                        break;
                }
            }
        }

        public static string GetStartTimeInput()
        {
            string format = "dd/MM/yyyy HH:mm";

            Console.WriteLine("Please input the date and the hour in which you started the coding session in the following format: dd/mm/yyyy HH:MM \n\nThe hour MUST be in 24 hour format. \n\nType M if you want to get back to the main menu.");

            string date = Console.ReadLine();

            if (date == "M") 
            {
                GetUserInput();
                Console.Clear();
            }

            if (!DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid format");
                GetStartTimeInput();
            }

            return date;
            
        }

        public static string GetEndTimeInput()
        {
            string format = "dd/MM/yyyy HH:mm";

            Console.WriteLine("Please input the date and the hour in which the coding session ended in the following format: dd/mm/yyyy HH:MM \n\nThe hour MUST be in 24 hour format. \n\nType M if you want to get back to the main menu.");

            string date = Console.ReadLine();

            if (date == "M")
            {
                GetUserInput();
                Console.Clear();
            }

            if (!DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid format");
                GetEndTimeInput();

            }

            return date;

        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();

            if (numberInput == "M")
            {
                GetUserInput();
                Console.Clear();
            }

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid number. Type M to return to the main menu or try again.\n\n");
                numberInput = Console.ReadLine();
                if (numberInput == "M") GetUserInput();
            }

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }
    }
}
