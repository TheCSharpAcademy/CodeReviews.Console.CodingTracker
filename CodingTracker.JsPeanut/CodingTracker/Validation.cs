using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    class Validation
    {
        public static void ValidateDate(string date, Func<string> Function, string format)
        {
            if (!DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid format");
                Function();
            }
        }

        public static void ValidateNumber(string numberInput, Action GetUserInput)
        {
            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid number. Type M to return to the main menu or try again.\n\n");
                numberInput = Console.ReadLine();
                if (numberInput == "M") GetUserInput();
            }
        }

        public static void ValidateDuration(string startTime, string endTime, string format, Func<string> GetStartTimeInput, Func<string> GetEndTimeInput)
        {
            if (!((DateTime.ParseExact(endTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None) < DateTime.ParseExact(startTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None))))
            {
                string durationMessage = CodingController.CalculateDuration(startTime, endTime);
            }
            else
            {
                Console.WriteLine("Invalid format. The time in which your session ended, was before your session even started.");
                GetStartTimeInput();
                GetEndTimeInput();
            }
        }
    }
}