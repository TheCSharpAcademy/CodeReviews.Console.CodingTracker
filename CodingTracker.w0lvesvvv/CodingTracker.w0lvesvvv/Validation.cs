using System.Globalization;

namespace CodingTracker.w0lvesvvv
{
    public static class Validation
    {
        public readonly static string DateTimeFormat = "dd/MM/yyyy HH:mm";

        public static bool ValidateNumber(string number, out int parsedNumber)
        {
            if (int.TryParse(number, out parsedNumber))
            {
                return true;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid input.");
            return false;
        }

        public static bool ValidateDateTimeString(string date) {

            if (DateTime.TryParseExact(date, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return true;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid input.");
            return false;
        }
    
        public static bool ValidateCorrectDateTimes(CodingSession codingSession)
        {
            if (codingSession.GetDuration() > 0) return true;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid dates.");
            return false;
        }
    }
}
