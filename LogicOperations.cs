using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTracker
{
    public static class LogicOperations
    {
        public static DateTime ConstructDateTime(string timeInput, string dateInput) => DateTime.Parse(dateInput + " " + timeInput);

        public static TimeSpan CalculateDuration(DateTime startTime, DateTime endTime) => startTime - endTime;

        public static TimeSpan CalculateBreaks(DateTime startTime, DateTime endTime, TimeSpan duration) => startTime - endTime + duration;

        public static TimeSpan AverageDuration(List<CodingSession> list)
        {
            TimeSpan total = TimeSpan.Zero;

            foreach (var session in list)
            {
                total += session.Duration;
            }

            long averageTicks = total.Ticks / list.Count; //tick units

            return TimeSpan.FromTicks(averageTicks);
        }
        public static TimeSpan TotalDuration(List<CodingSession> list)
        {
            TimeSpan total = TimeSpan.Zero;

            foreach (var session in list)
            {
                total += session.Duration;
            }

            return total;
        }
        public static int GetWeekNumber(DateTime date)
        {
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            Calendar calendar = cultureInfo.Calendar;
            CalendarWeekRule weekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
            DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            return calendar.GetWeekOfYear(date, weekRule, firstDayOfWeek);
        }
    }
}
