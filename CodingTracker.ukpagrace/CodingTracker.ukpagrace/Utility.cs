using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.ukpagrace
{
    internal class Utility
    {

        public TimeSpan GetDuration(DateTime startDate, DateTime endDate)
        {
            return endDate - startDate;
        }

        public string FormatTimeSpan(TimeSpan duration)
        {
            List<string> parts = new List<string>();
            if (duration.Days > 0)
            {
                parts.Add($"{duration.Days} {(duration.Days == 1 ? "day" : "days")}");
            }
            if (duration.Hours > 0)
            {
                parts.Add($"{duration.Hours} {(duration.Hours == 1 ? "hour" : "hours")}");
            }
            if (duration.Minutes > 0)
            {
                parts.Add($"{duration.Minutes} {(duration.Minutes == 1 ? "minute" : "minutes")}");
            }
            if (duration.Seconds > 0)
            {
                parts.Add($"{duration.Seconds} {(duration.Seconds == 1 ? "second" : "seconds")}");
            }
            return string.Join(", ", parts);
        }
    }
}
