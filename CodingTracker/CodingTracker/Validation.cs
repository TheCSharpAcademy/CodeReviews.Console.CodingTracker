using System;
using System.Globalization;

using System.Text.RegularExpressions;

namespace CodingTracker
{
    internal static class Validation
    {
        public static bool ValidateStringToDateTime(string? dateTime, out DateTime parsedDateTime)
        {
            return DateTime.TryParseExact(
                            Regex.Replace(dateTime?.Trim() ?? "", (@"\s+"), (" ")).Replace(".", "-"), // ?? "" crash safe - Regex.Replace(input, pattern, replacement)
                            "dd-MM-yyyy HH:mm:ss",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out parsedDateTime);
        }

        public static bool CheckStringToInt(string? input, out int value)
        {
            bool check = int.TryParse(input, out value);
            return check;
        }

        public static bool IsEndTimeValid(DateTime start, DateTime end)
        {
            return end > start;
        }
    }
}
