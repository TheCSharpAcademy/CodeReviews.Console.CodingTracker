namespace CodingTracker
{
    internal class CoddingSession
    {
        public long Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long Duration { get; }

        public CoddingSession(long id, DateTime startTime, DateTime endTime)
        {
            Id = id;
            StartTime = startTime;
            EndTime = endTime;
            Duration = CalculateDuration();
        }

        public long CalculateDuration() { 
            return (long)(EndTime - StartTime).TotalSeconds;
        }
    }
}
