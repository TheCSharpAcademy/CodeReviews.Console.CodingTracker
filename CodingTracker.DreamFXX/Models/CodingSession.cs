namespace CodingTracker.DreamFXX.Models
{
    public class CodingSession
    {
        public int Id { get; set; }

        public string Date { get; set; } = string.Empty;

        public string StartTime { get; set; } = string.Empty;

        public string EndTime { get; set; } = string.Empty;

        public string Duration { get; set; } = string.Empty;
    }
}
