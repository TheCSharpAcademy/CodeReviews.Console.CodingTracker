using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

        internal static bool DateEntryValidation(string dateInput)
        {
            bool isValidDate = false;
            string requiredFormat = @"\d{2}-\d{2}-\d{2}";

            if (Regex.IsMatch(dateInput, requiredFormat) && dateInput.Length == 8)
            {
                int dayNumber = Convert.ToInt32(dateInput[..2]);
                int monthNumber = Convert.ToInt32(dateInput.Substring(3, 2));

                if (IsWithinRange(01, 31, dayNumber) && IsWithinRange(01, 12, monthNumber))
                {
                    isValidDate = true;
                }
            }
            return isValidDate;
        }

        internal static bool TimeEntryValidation(string timeInput)
        {
            bool isValidTime = false;
            string requiredTimeFormat = @"\d{2}:\d{2}";

            if (Regex.IsMatch(timeInput, requiredTimeFormat) && timeInput.Length == 5)
            {
                int hour = Convert.ToInt32(timeInput[..2]);
                int minute = Convert.ToInt32(timeInput.Substring(3, 2));

                if (IsWithinRange(0, 23, hour) && IsWithinRange(0, 59, minute))
                {
                    isValidTime = true;
                }
            }
            return isValidTime;
        }

        internal static bool IsWithinRange(int minValue, int maxValue, int value)
        {
            return value >= minValue && value <= maxValue;
        }
    }
}
