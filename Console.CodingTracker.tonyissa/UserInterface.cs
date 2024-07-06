using CodingTracker.Dates;
using CodingTracker.Database;
using CodingTracker.Input;
using System.Diagnostics;

namespace CodingTracker.UserInterface
{
    public static class UIHelper
    {

        public static void InitMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to my coding tracker!");
            Console.WriteLine("Press C to create a record, U to update, D to delete, V to view all, R to record, and 0 to exit.");

            var input = Console.ReadLine() ?? "0";
            input = input.Trim().ToLower();

            switch (input)
            {
                case "0":
                    Environment.Exit(0);
                    break;
                case "c":
                    InitCreateMenu();
                    break;
                case "d":
                    InitDeleteMenu();
                    break;
                case "u":
                    break;
                case "v":
                    Console.Clear();
                    PrintAllEntries();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                case "r":
                    break;
                default:
                    Console.WriteLine("Input not recognized. Please try again.");
                    break;
            }
        }

        public static void PrintAllEntries()
        {
            var results = DatabaseController.GetList();

            foreach (var result in results)
            {
                var start = DateTime.Parse(result.Start!);
                var end = DateTime.Parse(result.End!);
                var (hours, minutes) = DateHelper.GetTotalTime(start, end);

                Console.WriteLine($"{result.ID}. Total: {hours} hours and {minutes} minutes, Start: {start}, End: {end}");
            }
        }

        public static void InitCreateMenu()
        {
            Console.Clear();

            var result = InputHelper.GetAllDateTimes();

            if (result == null) return;

            var (date1, date2) = result.Value;
            DatabaseController.Insert(date1, date2);
            Console.WriteLine("Item inserted successfully. Press any key to continue...");
            Console.ReadKey();
        }

        public static void InitDeleteMenu()
        {
            while (true)
            {
                Console.Clear();
                PrintAllEntries();
                Console.WriteLine("Please input the number ID of the entry you would like to delete, or type 0 to quit.");
                var input = Console.ReadLine() ?? "0";

                if (input == "0") return;

                Console.WriteLine("Are you sure? Enter Y to confirm or anything else to go back.");
                var confirmation = Console.ReadLine() ?? "";

                if (!(confirmation.Trim().ToLower() == "y")) continue;
                else if (DatabaseController.Delete(input))
                {
                    Console.WriteLine("Item successfully deleted! Press any key to continue...");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    Console.WriteLine("Input not a valid ID. Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }
}