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
                CalculateDuration();
            }
        }
        public DateTime End
        {
            get => _end;
            set
            {
                _end = value;
                CalculateDuration();
            }
        }
        public double Duration { get; private set; }

        public CodingSession() { }
        public CodingSession(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        private void CalculateDuration()
        {
            Duration = Math.Ceiling((End - Start).TotalSeconds);
        }
    }
}
