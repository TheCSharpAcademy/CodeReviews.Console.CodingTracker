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

        public double Duration
        {
            get
            {
                return Math.Round(CalculateDuration(), 0);
            }
        }
        public double CalculateDuration()
        {

            return (EndTime - StartTime).TotalMinutes;
        }
    }
}
