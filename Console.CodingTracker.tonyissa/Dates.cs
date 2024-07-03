using System.Globalization;

namespace CodingTracker.Dates
{
    public static class DateHelper
    {

        public static bool ValidateDateInput(string input)
        {
            return DateTime.TryParseExact(input, "MM/dd/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var result);
        }

        public static bool ValidateTimeInput(string input)
        {
            return DateTime.TryParseExact(input, "HH:mm", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var result);
        }

        public static bool CompareDates(DateTime date1, DateTime date2)
        {
            var comparison = DateTime.Compare(date1, date2);

            if (comparison >= 0)
            {
                return false;
            }

            return true;
        }

        public static string GetCurrentDate()
        {
            return DateTime.Now.Date.ToString("d");
        }

        public static string GetCurrentTime()
        {
            return DateTime.Now.TimeOfDay.ToString("t");
        }
    }
}