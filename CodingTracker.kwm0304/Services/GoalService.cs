using CodingTracker.kwm0304.Models;
using CodingTracker.kwm0304.Repositories;

public class GoalService
{
  private readonly GoalRepository _goalRepository;

  public GoalService()
  {
    _goalRepository = new GoalRepository();
  }

  public void CreateGoal(Goal goal)
  {
    _goalRepository.CreateGoal(goal);
  }

  public List<Goal> GetAllGoals()
  {
    return _goalRepository.GetAllGoals()!;
  }

  public List<Goal> GetActiveGoals()
  {
    return _goalRepository.GetActiveGoals()!;
  }

  public void SaveGoal(Goal goal)
  {
    _goalRepository.SaveGoal(goal);
  }

  public void UpdateGoalsOnComplete(CodingSession session)
  {
    var activeGoals = GetActiveGoals() ?? [];
    if (activeGoals.Count > 0)
    {
      foreach (var goal in activeGoals)
      {
        goal.AddSession(session);
        SaveGoal(goal);
      }
    }
  }

  public void UpdateGoalsOnDelete(CodingSession session)
  {
    var activeGoals = GetActiveGoals() ?? [];
    if (activeGoals.Count > 0)
    {
      foreach (var goal in activeGoals)
      {
        goal.DeleteSession(session);
        SaveGoal(goal);
      }
    }
  }
}
