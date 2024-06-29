using Spectre.Console;
using System;
using System.Globalization;
namespace CodingTracker
{
    public class UserInput
    {
        public string Duration(string startDate, string endDate)
        {
            if (DateTime.TryParseExact(startDate, "yyyy.MM.dd.", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime start) &&
                DateTime.TryParseExact(endDate, "yyyy.MM.dd.", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime end))
            {
                if (end < start)
                {
                    return "";
                }

                TimeSpan duration = end - start;

                return $"{duration.Days}";
            }
            else
            {
                return "";
            }
        }
    }
}
