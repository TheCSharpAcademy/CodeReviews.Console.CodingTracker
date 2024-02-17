namespace CodingTracker
{
    public class Goal
    {
        public int Id { get; set; }

        public TimeSpan GoalValue { get; set; }

        public DateTime AddedDate { get; set; }

        public TimeSpan ProgressRemaining { get; set; }
    }
}
