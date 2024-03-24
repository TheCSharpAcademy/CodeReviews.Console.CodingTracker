namespace CodingTracker.JaegerByte
{
    internal class CodingSession
    {
        public int ID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration
        {
            get
            {
                return EndTime.TimeOfDay - StartTime.TimeOfDay;
            }
        }
    }
}
