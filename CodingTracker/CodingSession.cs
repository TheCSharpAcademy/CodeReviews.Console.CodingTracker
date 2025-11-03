namespace CodingTracker
{
    public class CodingSession
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double Duration { get; set; }
        private TimeSpan Elapsed;

        public CodingSession() { }
        public CodingSession(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
            UpdateDuration();
        }

        public void UpdateDuration()
        {
            Duration = Math.Round((End - Start).TotalSeconds, 1);
            Elapsed = TimeSpan.FromSeconds(Duration);
        }

        public string DurationToString()
        {
            return Elapsed.TotalHours >= 1 ? Elapsed.ToString(@"hh\:mm\:ss") : Elapsed.ToString(@"mm\:ss");
        }
    }
}
