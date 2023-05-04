namespace CodeTracker
{
    internal class CodingSession
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly TimeStart { get; set; }
        public TimeOnly TimeEnd { get; set; }
        public TimeSpan TimeSpan { get; set; }
    }   
}
