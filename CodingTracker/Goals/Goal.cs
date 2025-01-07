internal class Goal
{
    public int TotalHours { get; set; }
    public int AverageHours { get; set; }
    public DateTime SetGoalDate { get; set; }

    public bool HasDefaultValues()
    {
        return TotalHours == 0 && AverageHours == 0 && SetGoalDate == DateTime.Parse("1000-01-01");
    }
}
