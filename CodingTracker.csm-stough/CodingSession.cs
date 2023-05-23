namespace CodeTracker.csm_stough
{
    public class CodingSession
    {
        public int id;
        public DateTime startTime;
        public DateTime endTime;
        public TimeSpan duration;

        public CodingSession(int id, DateTime startTime, DateTime endTime, TimeSpan duration)
        {
            this.id = id;
            this.startTime = startTime;
            this.endTime = endTime;
            this.duration = duration;
        }
    }
}
