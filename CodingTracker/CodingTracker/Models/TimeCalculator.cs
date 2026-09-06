using System;

namespace CodingTracker.Model
{
    internal static class TimeCalculator
    {
        internal static TimeSpan GetTimeSinceStart(DateTime startDateTime, DateTime currentDateTime)
        {
            return currentDateTime - startDateTime;
        }
    }
}
