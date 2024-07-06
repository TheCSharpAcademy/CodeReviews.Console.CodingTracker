using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodingTracker.Dates
{
    public static class DateHelper
    {

        public static bool ValidateDateFormat(string input)
        {
            return DateTime.TryParseExact(input, "M/d/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _);
        }

        public static bool ValidateTimeFormat(string input)
        {
            string[] formats = ["HH:mm", "h:mm tt"];
            return DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _);
        }

        public static bool CompareDates(DateTime date1, DateTime date2)
        {
            return DateTime.Compare(date1, date2) < 0;
        }

        public static string GetCurrentDate()
        {
            return DateTime.Now.Date.ToString("d");
        }

        public static string GetCurrentTime()
        {
            return DateTime.Now.TimeOfDay.ToString("t");
        }

        public static (int, int) GetTotalTime(DateTime start, DateTime end)
        {
            var total = end - start;
            return ((int)total.TotalHours, total.Minutes);
        }
    }
}