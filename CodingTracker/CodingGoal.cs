namespace CodingTracker
{
    public class CodingGoal
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Duration { get; set; }
        public int IsFinished { get; set; }
        public int IsAchieved { get; set; }
        public int RemainingDays => (End - DateTime.Now).Days + 1;

        public CodingGoal() { }
        public CodingGoal(DateOnly start, DateOnly end, int duration)
        {
            Start = start.ToDateTime(TimeOnly.MinValue);
            End = end.ToDateTime(TimeOnly.MaxValue);
            Duration = duration;
            IsFinished = 0;
            IsAchieved = 0;
        }

        public double GetRemainingDuration(double totalDuration)
        {
            return (Duration * 3600.0) - totalDuration;
        }

        public double GetDailyAverageNeeded(double remainingDuration)
        {
            return remainingDuration / RemainingDays;
        }

    }
}
