namespace CodingTracker
{
    public class CodingSession
    {
        public int Id { get; set; }
        private DateTime _start;
        private DateTime _end;
        public DateTime Start
        {
            get => _start;
            set
            {
                _start = value;
                UpdateDuration();
            }
        }
        public DateTime End
        {
            get => _end;
            set
            {
                _end = value;
                UpdateDuration();
            }
        }
        public double Duration { get; set; }

        public CodingSession() { }
        public CodingSession(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        private void UpdateDuration()
        {
            Duration = Math.Ceiling((End - Start).TotalSeconds);
        }
    }
}
