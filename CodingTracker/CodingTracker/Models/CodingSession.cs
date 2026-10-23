namespace CodingTracker.Models
{
    internal class CodingSession
    {
        public required int ID { get; set; }
        public required string StartTime { get; set; }
        public required string EndTime { get; set; }
        public required string Duration { get; set; }
    }
}
