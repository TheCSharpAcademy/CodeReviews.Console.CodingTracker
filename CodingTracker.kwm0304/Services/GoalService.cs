using CodingTracker.kwm0304.Models;
using CodingTracker.kwm0304.Repositories;

namespace CodingTracker.kwm0304.Services;

public class GoalService
{
  private readonly GoalRepository _repository;
  public GoalService()
  {
    _repository = new GoalRepository();
  }
  public void CreateGoal(Goal goal)
  {
    _repository.CreateGoal(goal);
  }
  public Goal GetGoalById(int id)
  {
    return _repository.GetGoalById(id)!;
  }
  public List<Goal> GetAllGoals()
  {
    return _repository.GetAllGoals()!;
  }
  public List<Goal> GetCompletedGoals()
  {
    return _repository.GetCompletedGoals();
  }
  
  public List<Goal> GetActiveGoals()
  {
    return _repository.GetActiveGoals()!;
  }

  public void UpdateActiveGoals(TimeSpan sessionLength)
  {
    List<Goal> goals = _repository.GetActiveGoals()!;

    foreach (Goal goal in goals)
    {
      int update = (int)sessionLength.TotalHours;
      _repository.UpdateGoalProgress(goal.GoalId, update);
      goal.Progress += update;
      goal.Accomplished = goal.CalculateProgressPercentage() >= 100;
      if (goal.Accomplished == true)
      {
        _repository.UpdateGoalCompletion(goal.GoalId, true);
      }
    }
  }

  public void SaveGoal(Goal goal)
  {
    _repository.SaveGoal(goal);
  }
}