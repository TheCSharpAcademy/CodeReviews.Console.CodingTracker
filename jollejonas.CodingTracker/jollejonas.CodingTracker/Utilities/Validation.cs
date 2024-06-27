using System;
using System.Globalization;
using System.Linq;

namespace jollejonas.CodingTracker.Utilities
{
    public class Validation
    {
        public static string CheckStartTime(string startTimeInput)
        {
            if (string.IsNullOrWhiteSpace(startTimeInput))
            {
                return "Start time cannot be empty.";
            }

            bool isValidStartTime = DateTime.TryParseExact(startTimeInput, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var startTime);
            if (!isValidStartTime)
            {
                return "Invalid start time format. Please enter the start time in the format dd-MM-yyyy HH:mm.";
            }
            else if (startTime > DateTime.Now)
            {
                return "Start time cannot be in the future.";
            }
            return null;
        }

        public static string CheckEndTime(string endTimeInput, string startTimeInput)
        {
            if (string.IsNullOrWhiteSpace(endTimeInput))
            {
                return "End time cannot be empty.";
            }

            if (!DateTime.TryParseExact(startTimeInput, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var startTime))
            {
                return "Invalid start time format. Please enter the start time in the format dd-MM-yyyy HH:mm.";
            }

            bool isValidEndTime = DateTime.TryParseExact(endTimeInput, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var endTime);
            if (!isValidEndTime)
            {
                return "Invalid end time format. Please enter the end time in the format dd-MM-yyyy HH:mm.";
            }
            else if (endTime > DateTime.Now)
            {
                return "End time cannot be in the future.";
            }
            else if (endTime <= startTime)
            {
                return "End time must be after the start time.";
            }
            return null;
        }
        public static int CheckYear(string yearInput)
        {
            int year = 0;
            while (!int.TryParse(yearInput, out year) || year < 1900 || year > DateTime.Now.Year)
            {
                Console.WriteLine("Invalid year. Please enter a year between 1900 and the current year.");
                yearInput = Console.ReadLine();
            }

            return year;
        }

        public static int CheckWeekNumber(string weekNumberInput)
        {
            int weekNumber = 0;
            while (!int.TryParse(weekNumberInput, out weekNumber) || weekNumber < 1 || weekNumber > 53)
            {
                Console.WriteLine("Invalid week number. Please enter a week number between 1 and 53.");
                weekNumberInput = Console.ReadLine();
            }
            return weekNumber;
        }

        public static int CheckWeeks(string weeksInput)
        {
            int weeks = 0;
            while (!int.TryParse(weeksInput, out weeks) || weeks < 1)
            {
                Console.WriteLine("Invalid number of weeks. Please enter a number greater than 0.");
                weeksInput = Console.ReadLine();
            }
            return weeks;
        }

        public static int CheckId(string idInput)
        {
            int id = 0;
            while (!int.TryParse(idInput, out id) || id < 1)
            {
                Console.WriteLine("Invalid id. Please enter a number greater than 0.");
                idInput = Console.ReadLine();
            }
            return id;
        }
    }
}
