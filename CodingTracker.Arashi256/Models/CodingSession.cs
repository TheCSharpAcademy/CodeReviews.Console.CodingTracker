namespace CodingTracker.Arashi256.Models
{
    internal class CodingSession
    {
        public int? Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Duration { get; set; }
    }
}
