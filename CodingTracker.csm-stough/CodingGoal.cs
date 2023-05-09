namespace CodeTracker.csm_stough
{
    public class CodingGoal
    {
        public int Id;
        public DateTime Start;
        public DateTime End;
        public TimeSpan CurrentHours;
        public TimeSpan TargetHours;

        public CodingGoal(int id, DateTime start, DateTime end, TimeSpan currentHours, TimeSpan targetHours)
        {
            Id = id;
            Start = start;
            End = end;
            CurrentHours = currentHours;
            TargetHours = targetHours;
        }
    }
}
