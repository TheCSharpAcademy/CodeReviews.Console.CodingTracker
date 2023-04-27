namespace CodeTracker
{
    internal class CodingSession
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public TimeSpan TimeSpan { get; set; }
    }   
}
