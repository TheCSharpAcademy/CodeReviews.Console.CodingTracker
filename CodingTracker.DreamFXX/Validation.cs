using System.Globalization;

namespace CodingTracker.DreamFXX
{
    internal class Validation
    {
        private TimeOnly _startTime;
        private TimeOnly _endTime;

        public bool IsValidDate(string date)
        {
            return DateOnly.TryParseExact(date, "dd-MM-yy", out DateOnly Date);
        }

        public bool IsValidStartTime(string startTime)
        {
            return TimeOnly.TryParseExact(startTime, "HH:mm", null, DateTimeStyles.None, out _startTime);
        }

        public bool IsValidEndTime(string endTime)
        {
            return TimeOnly.TryParseExact(endTime, "HH:mm", null, DateTimeStyles.None, out _endTime);
        }

        public bool IsValidDuration()
        {
            return _startTime.CompareTo(_endTime) < 0;
        }
    }
}
