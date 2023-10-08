namespace CodingTracker.AndreasGuy54
{
    internal class CodingSession
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        private TimeSpan myTime;

        public TimeSpan Duration
        {
            get { return myTime; }
            set { myTime = EndTime - StartTime; }
        }
    }
}
