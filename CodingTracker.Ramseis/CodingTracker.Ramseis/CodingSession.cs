namespace CodingTracker.Ramseis
{
    internal class CodingSession
    {
        public int ID { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public TimeSpan Duration()
        {
            return End - Start;
        }
    }
}
