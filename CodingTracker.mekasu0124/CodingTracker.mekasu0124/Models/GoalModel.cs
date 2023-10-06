namespace CodingTracker.Models;

public class Goal
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public double? DaysToGoal { get; set; }
    public int? HoursPerDay { get; set; }
    public string? Achieved { get; set; }
}
