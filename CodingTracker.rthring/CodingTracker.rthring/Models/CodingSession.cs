namespace CodingTracker.rthring.Models
{
    internal class CodingSession
    {
        public int id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
