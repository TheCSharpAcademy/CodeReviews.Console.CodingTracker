using CodingTracker.Dates;
using CodingTracker.Database;
using CodingTracker.Input;

namespace CodingTracker.UserInterface
{
    public static class UIHelper
    {

        public static void InitMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to my coding tracker!");
                Console.WriteLine("Press C to create a record, U to update, D to delete, V to view all, R to record, and 0 to exit.");

                var input = Console.ReadLine() ?? "0";
                input = input.Trim().ToLower();

                switch (input)
                {
                    case "0":
                        break;
                    case "c":
                        InitCreateMenu();
                        break;
                    case "d":
                        break;
                    case "u":
                        break;
                    case "v":
                        PrintAllEntries();
                        break;
                    case "r":
                        break;
                    default:
                        Console.WriteLine("Input not recognized. Please try again.");
                        continue;
                }
            }
        }

        public static void InitCreateMenu()
        {
            Console.Clear();
            
            try
            {
                var result = GetAllDateTimes();

                if (result == null) return;

                var (date1, date2) = result.Value;
                DatabaseController.Insert(date1, date2);
                Console.WriteLine("Item inserted successfully. Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Please try again. Press any key to continue.");
                Console.ReadKey();
                InitCreateMenu();
                return;
            }
        }

        public static void PrintAllEntries()
        {
            var results = DatabaseController.GetList();
            var index = 1;

            foreach (var result in results)
            {
                var start = DateTime.Parse(result.Start!);
                var end = DateTime.Parse(result.End!);
                var (hours, minutes) = DateHelper.GetTotalTime(start, end);

                Console.WriteLine($"{index++}. Start: {start}, End: {end}, Total: {hours}H {minutes}M");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

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