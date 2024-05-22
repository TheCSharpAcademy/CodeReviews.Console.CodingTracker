namespace CodingTracker.Models
{
    public class CodingSession
    {
        public int Id {get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now;
        public TimeSpan Duration { get; set; } = DateTime.Now - DateTime.Now;
    }
}
