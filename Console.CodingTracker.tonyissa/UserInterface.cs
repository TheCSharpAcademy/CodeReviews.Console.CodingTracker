using CodingTracker.Dates;
using CodingTracker.Database;
using CodingTracker.Input;
using System.Diagnostics;
using CodingTracker.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                    InitUpdateMenu();
                    break;
                case "v":
                    Console.Clear();
                    PrintAllEntries(false);
                    break;
                case "r":
                    InitRecordMenu();
                    break;
                default:
                    Console.WriteLine("Input not recognized. Please try again.");
                    break;
            }
        }

        public static List<CodingSession>? PrintAllEntries(bool returnList)
        {
            var results = DatabaseController.GetList();

            foreach (var result in results)
            {
                var start = DateTime.Parse(result.Start!);
                var end = DateTime.Parse(result.End!);
                var (hours, minutes) = DateHelper.GetTotalTime(start, end);

                Console.WriteLine($"{result.ID}. Total: {hours} hours and {minutes} minutes, Start: {start}, End: {end}");
            }

            if (returnList && results.Any()) return results.ToList();
            else if (!results.Any())
            {
                Console.WriteLine("No entries found.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            return null;
        }

        public static void InitCreateMenu()
        {
            Console.Clear();

            var result = InputHelper.GetAllDateTimes();

            if (result == null) return;

            var (date1, date2) = result.Value;
            DatabaseController.Insert(date1.ToString(), date2.ToString());
            Console.WriteLine("Session inserted successfully. Press any key to continue...");
            Console.ReadKey();
        }

        public static void InitDeleteMenu()
        {
            while (true)
            {
                Console.Clear();
                var list = PrintAllEntries(true);

                if (list == null) return;

                Console.WriteLine("Please input the number ID of the entry you would like to delete, or type 0 to quit.");
                var index = InputHelper.CheckIndex(list);

                if (index == null) return;
                else if (index == -1) continue;

                Console.WriteLine("Are you sure? Enter Y to confirm or anything else to go back.");
                var confirmation = Console.ReadLine() ?? "";

                if (confirmation.Trim().ToLower() != "y") continue;

                DatabaseController.Delete(index.Value);
                Console.WriteLine("Session deleted successfully. Press any key to continue...");
                Console.ReadKey();
            }
        }

        public static void InitUpdateMenu()
        {
            while (true)
            {
                Console.Clear();
                var list = PrintAllEntries(true);

                if (list == null) return;

                Console.WriteLine("Please input the number ID of the entry you would like to update, or type 0 to quit.");
                var index = InputHelper.CheckIndex(list);

                if (index == null) return;
                else if (index == -1) continue;

                var result = InputHelper.GetAllDateTimes();

                if (result == null) return;

                Console.WriteLine("Are you sure? Enter Y to confirm or anything else to go back.");
                var confirmation = Console.ReadLine() ?? "";

                if (confirmation.Trim().ToLower() != "y") continue;

                var (date1, date2) = result.Value;
                DatabaseController.Update(index.Value, date1.ToString(), date2.ToString());
                Console.WriteLine("Session updated successfully. Press any key to continue...");
                Console.ReadKey();
            }
        }

        public static void InitRecordMenu()
        {
            Console.Clear();
            Console.WriteLine("You can record your coding time here. Press any key to start the stopwatch, or press 0 to go back");
            var result1 = Console.ReadKey();

            if (result1.KeyChar == '0') return;

            var startDate = DateHelper.GetCurrentDate();
            var startTime = DateHelper.GetCurrentTime();

            Console.WriteLine("Press any key to stop the stopwatch and record your results, or press 0 to go back.");
            var result2 = Console.ReadKey();

            if (result2.KeyChar == '0') return;

            var endDate = DateHelper.GetCurrentDate();
            var endTime = DateHelper.GetCurrentTime();

            DatabaseController.Insert($"{startDate} {startTime}", $"{endDate} {endTime}");
            Console.WriteLine("Session recorded successfully. Press any key to continue...");
            Console.ReadKey();
        }
    }
}