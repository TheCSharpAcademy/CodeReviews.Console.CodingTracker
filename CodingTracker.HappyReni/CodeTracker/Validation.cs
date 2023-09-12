using System.Globalization;
using System.Text.RegularExpressions;

namespace CodeTracker
{
    internal static class Validation
    {
        public static DateTime ValidDateTime(string date)
        {
            string format = "yyyy-MM-dd HH:mm:ss";

            if (DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return DateTime.Parse(date);
            }

            throw new Exception();
        }
        public static int ValidYear(int year) => year >= 0 && year <= 9999 ? year : throw new Exception();
        public static string ValidWeek(string week)
        {
            Regex regex = new Regex(@"[0-9]{4}-(0[1-9]|[1-4][0-9]|5[0-2]");
            return regex.IsMatch(week) ? week : throw new Exception();
        }
        public static DateTime ValidDate(string date)
        {
            string format = "yyyy-MM-dd";

            if (DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return DateTime.Parse(date);
            }

            throw new Exception();
        }
    }
}
