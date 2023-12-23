namespace CodingTracker.Furiax.Model
{
	internal class CodingSession
	{
        public int Id { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public TimeSpan Duration { get; set; }
    }
}
