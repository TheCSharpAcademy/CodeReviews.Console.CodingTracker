using CodingTracker.Dates;

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
    }
}