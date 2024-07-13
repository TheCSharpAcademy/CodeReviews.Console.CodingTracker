using CodingTracker.kwm0304.Enums;

namespace CodingTracker.kwm0304.Models;

public class Goal
{
  public int GoalId { get; set; }
  public string GoalName { get; set; }
  public int TargetNumber { get; set; }
  public bool Accomplished { get; set; }
  public DateTime CreatedOn { get; set; }
  public DateRange AccomplishBy { get; set; }
  public List<CodingSession> Sessions { get; private set; }
  public Goal(string goalName, int target, DateRange range)
  {
    GoalName = goalName;
    TargetNumber = target;
    AccomplishBy = range;
    CreatedOn = DateTime.Now;
    Sessions = [];
  }
  public DateTime EndDate
  {
    get
    {
      return AccomplishBy switch
      {
        DateRange.Week => CreatedOn.AddDays(7),
        DateRange.Month => CreatedOn.AddMonths(1),
        DateRange.Year => CreatedOn.AddYears(1),
        _ => CreatedOn
      };
    }
  }

  //calculate progress method:  returns % of TargetNumber, totalTime = sum of Sessions -> totalTime/TargetNumber * 100
}
