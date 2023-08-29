using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.w0lvesvvv
{
    public static class Validation
    {
        public readonly static string dateTimeFormat = "dd/MM/yyyy HH:mm";

        public static bool validateNumber(string number, out int parsedNumber)
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

        public static bool validateDateTimeString(string date) {

            if (DateTime.TryParseExact(date, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return true;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid input.");
            return false;
        }
    
        public static bool validateCorrectDateTimes(CodingSession codingSession)
        {
            if (codingSession.getDuration() > 0) return true;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid dates.");
            return false;
        }
    }
}
