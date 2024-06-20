namespace CodingTrackerProgram.Model
{
    public class CodingSession
    {
        public int Id;
        public DateTime StartTime;
        public DateTime EndTime;
        public TimeSpan Duration
        {
            get
            {
                return CalculateDuration(StartTime, EndTime);
            }
        }

        public static TimeSpan CalculateDuration(DateTime startTime, DateTime endTime)
        {
            return endTime.Subtract(startTime);
        }

        public CodingSession(int id, DateTime startTime, DateTime endTime)
        {
            Id = id;
            StartTime = startTime;
            EndTime = endTime;
        }

        public static bool IsValidTimeRange(DateTime? startTime, DateTime? endTime)
        {
            return (
                startTime != null &&
                endTime != null &&
                endTime.Value >= startTime.Value
            );
        }
    }
}