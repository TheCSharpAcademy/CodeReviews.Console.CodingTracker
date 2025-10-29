namespace CodingTracker
{
    public class CodingSession
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double Duration { get; set; }

        public CodingSession() { }
        public CodingSession(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
            UpdateDuration();
        }

        public void UpdateDuration()
        {
            Duration = (End - Start).TotalSeconds;
        }
    }
}
