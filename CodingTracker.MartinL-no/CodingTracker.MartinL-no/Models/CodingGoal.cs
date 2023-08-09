namespace CodingTracker.MartinL_no.Models;

internal class CodingGoal
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Hours { get; set; }

    // Properties not saved/retreived from database
    public TimeSpan TimeCompleted { get; set; }
    public TimeSpan HoursPerDayToComplete { get; set; }

    public CodingGoal(DateTime startTime, DateTime endTime, int hours)
    {
        StartTime = startTime;
        EndTime = endTime;
        Hours = hours;
    }

    public CodingGoal(int id, DateTime startTime, DateTime endTime, int hours) : this(startTime, endTime, hours) 
    {
        Id = id;
    }
}