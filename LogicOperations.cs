using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTracker
{
    public static class LogicOperations
    {
        public static DateTime ConstructDateTime(string timeInput, string dateInput) => DateTime.Parse(dateInput + " " + timeInput);

        public static TimeSpan CalculateDuration(DateTime startTime, DateTime endTime) => startTime - endTime;

        public static TimeSpan CalculateBreaks(DateTime startTime, DateTime endTime, TimeSpan duration) => startTime-endTime + duration;
    }
}
