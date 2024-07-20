using CodingTracker.kwm0304.Enums;

namespace CodingTracker.kwm0304.Models
{
  public class Goal
  {
    public int GoalId { get; set; }
    public string GoalName { get; set; }
    public int TargetNumber { get; set; }
    public int Progress { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateRange AccomplishBy { get; set; }
    public List<CodingSession> Sessions { get; private set; }

    public Goal()
    {
      GoalName = string.Empty;
      Sessions = [];
    }

    public Goal(string name, int target, DateRange range, int progress)
    {
      GoalName = name;
      TargetNumber = target;
      Progress = progress;
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

    private bool _accomplished;
    public bool Accomplished
    {
      get { return CalculateProgressPercentage() >= 100; }
      set { _accomplished = value; }
    }

    public void AddSession(CodingSession session)
    {
      var exists = Sessions.FirstOrDefault(s => s.Id == session.Id);
      if (exists != null)
      {
        Sessions.Remove(exists);
        Progress -= (int)exists.SessionLength.TotalHours;
      }
      Sessions.Add(session);
      Progress += (int)session.SessionLength.TotalHours;
      Accomplished = CalculateProgressPercentage() >= 100;
    }

    public void DeleteSession(CodingSession session)
    {
      var exists = Sessions.FirstOrDefault(s => s.Id == session.Id);
      if (exists != null)
      {
        Sessions.Remove(exists);
        Progress -= (int)exists.SessionLength.TotalHours;
        Accomplished = CalculateProgressPercentage() >= 100;
      }
    }

    public double CalculateProgressPercentage()
    {
      return (Progress / (double)TargetNumber) * 100;
    }
  }
}