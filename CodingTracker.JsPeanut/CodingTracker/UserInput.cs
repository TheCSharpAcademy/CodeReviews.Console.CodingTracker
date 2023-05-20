using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            if (CodingController.Goals.Count != 0)
            {
                Console.WriteLine($"Reminder: You are {CodingController.Goals.First().GoalValue - CodingController.DisplayGoal()} hours away from completing your last goal (15 hours of coding time)!");
            }
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
                    case "G":
                        CodingController.SetGoal();
                        break;
                    case "T":
                        CodingController.GetAllGoalRecords();
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

            Validation.ValidateDate(date, GetStartTimeInput, format);

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

            Validation.ValidateDate(date, GetEndTimeInput, format);
            return date;

        }

        public static string GetGoalMeasureInput()
        {
            Console.WriteLine("Please type if you want your input to be measured in 'years', 'months', 'days', 'hour' or 'minutes'.");

            string goalTimePeriod = Console.ReadLine();

            return goalTimePeriod;
        }

        public static string GetGoalInput()
        {
            Console.WriteLine("Please type your goal. It must be a number.");

            string goalInput = Console.ReadLine();

            return goalInput;
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

            Validation.ValidateNumber(numberInput, GetUserInput);
            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }
    }
}
