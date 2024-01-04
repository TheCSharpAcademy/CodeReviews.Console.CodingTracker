using System.Globalization;

namespace CodingTracker
{
    internal static class Validation
    {
        private static DateTime ParseDate(string date)
        {
            DateTime output = DateTime.ParseExact(date, "dd-MM-yyyy HH:mm:ss",
                   CultureInfo.InvariantCulture, DateTimeStyles.None);

            return output;
        }
        internal static string CodingSessionDuration(string start, string end)
        {
            DateTime startDateTime = ParseDate(start);
            DateTime endDateTime = ParseDate(end);

            TimeSpan dif = endDateTime - startDateTime;

            return dif.ToString();
        }
        internal static bool ValidateDates(string start, string end)
        {
            DateTime startDateTime = ParseDate(start);
            DateTime endDateTime = ParseDate(end);

            if (startDateTime >= endDateTime) return false;
            return true;
        }
    }
}
