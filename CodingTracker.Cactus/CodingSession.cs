namespace CodingTracker.Cactus
{
    public class CodingSession
    {
        public CodingSession(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public CodingSession(int id, DateTime startTime, DateTime endTime)
        {
            Id = id;
            StartTime = startTime;
            EndTime = endTime;
        }

        public int Id { set; get; }

        public DateTime StartTime { set; get; }

        public DateTime EndTime { set; get; }

        public int Duration
        {
            get
            {
                return CalculateDuration();
            }
        }
        public int CalculateDuration()
        {
            return EndTime.Minute - StartTime.Minute;
        }
    }
}
