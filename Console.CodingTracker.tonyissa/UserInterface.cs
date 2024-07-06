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
                    //InitDeleteMenu();
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

            var result = GetAllDateTimes();

            if (result == null) return;

            var (date1, date2) = result.Value;
            DatabaseController.Insert(date1, date2);
            Console.WriteLine("Item inserted successfully. Press any key to continue...");
            Console.ReadKey();
        }

        //public static void InitDeleteMenu()
        //{
        //    while (true)
        //    {
        //        Console.Clear();
        //        PrintAllEntries();
        //        Console.WriteLine("Please input the number ID of the entry you would like to delete, or type 0 to quit.");
        //        var input = Console.ReadLine() ?? "";
        //        input = input.Trim().ToLower();

        //        if (input == "0") return;

        //        Console.WriteLine("Are you sure? Press Y to confirm or any other key to go back.");
        //        var confirmation = Console.ReadKey();

        //        if (confirmation.ToString().ToLower() == "y")
        //        {
        //            Console.WriteLine("yay");
        //            Console.ReadKey();
        //        }
        //    }
        //}

        public static (DateTime, DateTime)? GetAllDateTimes()
        {
            var startDate = InputHelper.GetDateInput(true);
            if (startDate == "0") return null;
            else if (startDate == "") startDate = DateHelper.GetCurrentDate();

            var startTime = InputHelper.GetTimeInput(true);
            if (startTime == "0") return null;
            else if (startTime == "") startTime = DateHelper.GetCurrentTime();

            var endDate = InputHelper.GetDateInput(false);
            if (endDate == "0") return null;
            else if (endDate == "") endDate = DateHelper.GetCurrentDate();

            var endTime = InputHelper.GetTimeInput(false);
            if (endTime == "0") return null;
            else if (endTime == "") endTime = DateHelper.GetCurrentTime();

            var date1 = DateTime.Parse($"{startDate} {startTime}");
            var date2 = DateTime.Parse($"{endDate} {endTime}");

            if (!DateHelper.CompareDates(date1, date2))
            {
                throw new ArgumentException("Date 1 equal or later than date 2.");
            }

            return (date1, date2);
        }
    }
}