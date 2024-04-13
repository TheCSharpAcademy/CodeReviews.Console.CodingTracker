using CodingTracker.Database.Models;

namespace CodingTracker.Database;

public class GoalTracker
{
  public int IsGoalCompleted(Goal goal, List<CodingSession> sessions)
  {
    int totalDuration = CalculateTotalDuration(goal.Start_Date, goal.End_Date, sessions);

    if (totalDuration >= goal.Target_Duration) return 1;
    else return 0;
  }

  private int CalculateTotalDuration(string startDate, string endDate, List<CodingSession> sessions)
  {
    List<CodingSession> sessionsWithinGoalPeriod = sessions.Where(session => Convert.ToDateTime(session.Start_Date) >= Convert.ToDateTime(startDate) && Convert.ToDateTime(session.End_Date) <= Convert.ToDateTime(endDate)).ToList();

    int totalDuration = sessionsWithinGoalPeriod.Sum(session => session.Duration);

    return totalDuration;
  }
}