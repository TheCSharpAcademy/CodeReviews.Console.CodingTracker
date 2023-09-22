using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodingTracker.TomDonegan
{
    internal class Validation
    {
        public static bool MenuValidation(string[] selectionOptions, string menuSelection)
        {
            return selectionOptions.Any(selection => selection == menuSelection);
        }

        internal static string DateEntryValidation(string prompt)
        {
            Console.WriteLine(prompt);

            string requiredFormat = @"\d{2}-\d{2}-\d{2}";

            while (true)
            {
                string dateInput = Console.ReadLine();

                if (Regex.IsMatch(dateInput, requiredFormat) && dateInput.Length == 8)
                {
                    int dayNumber = Convert.ToInt32(dateInput[..2]);
                    int monthNumber = Convert.ToInt32(dateInput.Substring(3, 2));

                    if (IsWithinRange(01,31,dayNumber) && IsWithinRange(01,12, monthNumber))
                    {
                        return dateInput;
                    }
                }
                Console.WriteLine("Invalid input. Please enter the date in DD-MM-YY format and within the valid range (DD: 01-31, MM: 01-12). Try again.");
            }
        }

        internal static string TimeEntryValidation(string prompt)
        {
            Console.WriteLine(prompt);            

            string requiredTimeFormat = @"\d{2}:\d{2}";

            while (true)
            {
                string timeInput = Console.ReadLine();

                if (Regex.IsMatch(timeInput, requiredTimeFormat) && timeInput.Length == 5)
                {
                    int hour = Convert.ToInt32(timeInput[..2]);
                    int minute = Convert.ToInt32(timeInput.Substring(3, 2));

                    if (IsWithinRange(0, 23, hour) && IsWithinRange(0, 59, minute))
                    {
                        return timeInput;
                    }
                }

                Console.WriteLine("Invalid input. Please enter the time in HH:MM format and within the valid range (HH: 00-23, MM: 00-59). Try again.");
            }
        }

        internal static bool IsWithinRange(int minValue, int maxValue, int value)
        {
            return value >= minValue && value <= maxValue;
        }
    }
}
