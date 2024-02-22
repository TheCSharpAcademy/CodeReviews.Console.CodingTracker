namespace CodingTracker.Dejmenek.Models
{
    public class CodingSession
    {
        public int Id { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public int Duration { get; set; }
    }
}
