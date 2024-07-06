using CodingTracker.Dates;
using CodingTracker.Database;

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
                Console.WriteLine("Press C to create a record, U to update, D to delete, V to view all, and 0 to exit.");

                var input = Console.ReadLine() ?? "0";
                input = input.Trim().ToLower();

                if (input == "0") return;
                else if (input == "c") InitCreateMenu();
                else if (input == "d") break;
                else if (input == "u") break;
                else if (input == "v") GetAllEntries();
                else
                {
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

        public static void GetAllEntries()
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
            var startDate = GetDateInput(true);
            if (startDate == "0") return null;
            else if (startDate == "") startDate = DateHelper.GetCurrentDate();

            var startTime = GetTimeInput(true);
            if (startTime == "0") return null;
            else if (startTime == "") startTime = DateHelper.GetCurrentTime();

            var endDate = GetDateInput(false);
            if (endDate == "0") return null;
            else if (endDate == "") endDate = DateHelper.GetCurrentDate();

            var endTime = GetTimeInput(false);
            if (endTime == "0") return null;
            else if (endTime == "") endTime = DateHelper.GetCurrentTime();

            var date1 = DateTime.Parse($"{startDate} {startTime}");
            var date2 = DateTime.Parse($"{endDate} {endTime}");

            if (!DateHelper.CompareDates(date1, date2))
            {
                throw new ArgumentException("Date 1 equal or later than date 2. Press any key to continue and try again.");
            }

            return (date1, date2);
        }

        public static string GetDateInput(bool start)
        {
            string s = start ? "start" : "end";
            Console.WriteLine($"Please input the {s} date in the M/D/YYYY format, type 0 to quit, or press enter for the current date.");

            while (true)
            {
                var input = Console.ReadLine() ?? "0";
                input = input.Trim().ToLower();

                if (input == "0" || input == string.Empty || DateHelper.ValidateDateFormat(input))
                {
                    return input;
                }

                Console.WriteLine("Invalid date.");
            }
        }

        public static string GetTimeInput(bool start)
        {
            string s = start ? "start" : "end";
            Console.WriteLine($"Please input the {s} time in either hh:mm tt (12-hour time) format, HH:MM (24-hour time) format, type 0 to quit, or press enter for the current time.");

            while (true)
            {
                var input = Console.ReadLine() ?? "0";
                input = input.Trim().ToLower();

                if (input == "0" || input == string.Empty || DateHelper.ValidateTimeFormat(input))
                {
                    return input;
                }

                Console.WriteLine("Invalid time.");
            }
        }
    }
}