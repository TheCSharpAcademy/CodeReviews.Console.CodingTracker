using System.Text.RegularExpressions;

namespace CodingTracker.TomDonegan
{
    internal class Validation
    {
        internal static bool MenuValidation(string[] selectionOptions, string menuSelection)
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

        internal static bool IsSecondTimeBeforeFirstTime(string[] sessionTimes)
        {
            DateTime firstTime = DateTime.ParseExact(sessionTimes[0], "HH:mm", null);
            DateTime secondTime = DateTime.ParseExact(sessionTimes[1], "HH:mm", null);

            return secondTime < firstTime;
        }

        internal static bool DoesIdExistInTable(string id)
        {
            bool idExists = false;

            List<CodingSession> sessionData = Database.ViewAllSQLiteDatabase();

            if (!Int32.TryParse(id, out _))
            {
                return idExists;
            }

            foreach (var session in sessionData)
            {
                if (session.Id.ToString() == (id))
                {
                    idExists = true;
                }
            }
            return idExists;
        }

        internal static bool IsWithinRange(int minValue, int maxValue, int value)
        {
            return value >= minValue && value <= maxValue;
        }

        internal static bool YesNoConfirm(string message)
        {
            return message == "y" || message == "n";
        }
    }
}
