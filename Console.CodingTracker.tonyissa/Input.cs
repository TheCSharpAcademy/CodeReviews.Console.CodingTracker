using CodingTracker.Dates;
using CodingTracker.Model;
using System.Collections.Generic;

namespace CodingTracker.Input
{
    public static class InputHelper
    {
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
                throw new ArgumentException("Date 1 equal or later than date 2.");
            }

            return (date1, date2);
        }

        public static int? CheckIndex(List<CodingSession> list)
        {
            if (!int.TryParse(Console.ReadLine(), out int index))
            {
                Console.WriteLine("Invalid input");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                return null;
            }
            else if (index == 0) return null;

            var result = list.FindIndex(item => item.ID == index);

            if (result == -1)
            {
                Console.WriteLine("Item not found.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return result;
            }

            return list[result].ID;
        }
    }
}