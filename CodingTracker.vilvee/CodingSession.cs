
using System.Globalization;

namespace CodingTracker.vilvee
{

    public class CodingSession
    {
        private DateTime _startTime;
        private DateTime? _endTime;


        public int Id { get; internal set; }

        public string? TotalSessionDuration { get; private set; }

        public DateTime StartTime
        {
            get => _startTime;
            set
            {
                if (value > _endTime && _endTime.HasValue)
                    throw new ArgumentException("Start time cannot be after end time.");
                _startTime = value;
            }
        }

        public DateTime? EndTime
        {
            get => _endTime;
            set
            {
                if (value.HasValue && value < _startTime)
                    throw new ArgumentException("End time cannot be before start time.");
                _endTime = value;
            }
        }
        public string Duration => CalculateDuration(_startTime, _endTime);

        public string FormattedStartTime => _startTime.ToString("dd/MM/yy HH:mm:ss", CultureInfo.InvariantCulture) ?? string.Empty;

        public string FormattedEndTime => _endTime?.ToString("dd/MM/yy HH:mm:ss", CultureInfo.InvariantCulture) ?? string.Empty;


        /// <summary>
        /// Default constructor
        /// </summary>
        public CodingSession()
        {


        }

        /// <summary>
        /// Constructor with start time
        /// </summary>
        /// <param name="starTime"></param>
        public CodingSession(DateTime starTime)
        {
            StartTime = starTime;

        }

        /// <summary>
        /// Constructor with both start and end time
        /// </summary>
        /// <param name="starTime"></param>
        /// <param name="endTime"></param>
        public CodingSession(DateTime starTime, DateTime? endTime) : this(starTime)
        {
            EndTime = endTime;
            SetTotalDuration(CalculateDuration(starTime, endTime));
        }

        internal void SetTotalDuration(string duration)
        {
            TotalSessionDuration = duration;
        }

        private static string CalculateDuration(DateTime startTime, DateTime? endTime)
        {
            if (!endTime.HasValue)
            {
                return "Ongoing";
            }

            if (startTime > endTime.Value)
            {
                return "ERROR: End date cannot be earlier than start date";
            }

            TimeSpan duration = endTime.Value - startTime;
            return FormatDuration(duration);
        }

        private static string FormatDuration(TimeSpan duration)
        {
            var parts = new List<string>();
            if (duration.Days > 0) parts.Add($"{duration.Days} day{(duration.Days > 1 ? "s" : "")}");
            if (duration.Hours > 0) parts.Add($"{duration.Hours} hour{(duration.Hours > 1 ? "s" : "")}");
            if (duration.Minutes > 0) parts.Add($"{duration.Minutes} minute{(duration.Minutes > 1 ? "s" : "")}");
            if (duration.Seconds > 0) parts.Add($"{duration.Seconds} second{(duration.Seconds > 1 ? "s" : "")}");

            return parts.Count > 0 ? string.Join(", ", parts) : "0 seconds";
        }
    }
}
