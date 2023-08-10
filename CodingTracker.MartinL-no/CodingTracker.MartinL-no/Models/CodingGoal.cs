namespace CodingTracker.MartinL_no.Models;

internal class CodingGoal
{
    public readonly int Id;
    public readonly DateTime StartTime;
    public readonly DateTime EndTime;
    public readonly int Hours;

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