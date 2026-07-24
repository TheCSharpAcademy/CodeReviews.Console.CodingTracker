using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CodingTracker.CSharpAcademy_Learner
{
    internal static class Validation
    {
        internal const string ViewingDateFormat = "dd/MM/yyyy HH:mm";
        internal const string DatabaseDateFormat = "dd/MM/yyyy HH:mm:ss";

        internal static bool IsValidDate(string dateString, out DateTime parsedDate)
        {
            return DateTime.TryParseExact(dateString, ViewingDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
        }

        internal static bool IsValidDateRange(DateTime startDate, DateTime endDate)
        {
            return endDate > startDate;
        }

        internal static int CalculateDurationInSeconds(DateTime startDate, DateTime endDate)
        {
            return (int)(endDate - startDate).TotalSeconds;
        }

        internal static string ShowDurationInFriendlyFormat(int durationInSeconds)
        {
            var timeSpan = TimeSpan.FromSeconds(durationInSeconds);

            var formattedDuration = "";

            if (timeSpan.TotalHours >= 1)
            {
                int totalHours = (int)timeSpan.TotalHours;
                formattedDuration += $"{totalHours} hour{(totalHours == 1 ? "" : "s")}, ";
            }

            if (timeSpan.Minutes >= 1)
            {
                int minutes = timeSpan.Minutes;
                formattedDuration += $"{minutes} minute{(minutes == 1 ? "" : "s")}, ";
            }

            int seconds = timeSpan.Seconds;
            formattedDuration += $"{seconds} second{(seconds == 1 ? "" : "s")}";

            return formattedDuration;
        }
    }
}
