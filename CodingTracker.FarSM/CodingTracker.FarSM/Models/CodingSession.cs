namespace CodingTracker.FarSM.Models
{
    internal class CodingSession
    {
        public int Id { get; set; }
        public DateTime date { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public TimeSpan duration { get; set; }
    }
}
