using CodingTracker.kwm0304.Data;
using CodingTracker.kwm0304.Models;
using Spectre.Console;

namespace CodingTracker.kwm0304.Repositories;

public class GoalRepository
{
    private readonly DbAction _dbAction;
    public GoalRepository(DbAction action)
    {
      _dbAction = action;
    }

    public Goal? GetGoalById(int id)
    {
      try
      {
        return _dbAction.GetGoalById(id);
      }
      catch (Exception e)
      {
        AnsiConsole.WriteException(e);
        return null;
      }
    }

    public List<Goal>? GetAllGoals()
    {
      try
      {
        List<Goal> goals = _dbAction.GetAllGoals()!;
        return goals;
      }
      catch (Exception e)
      {
        AnsiConsole.WriteException(e);
        return null;
      }
    }
    public void CreateGoal(Goal goal)
    {
      try
      {
        _dbAction.InsertGoal(goal);
      }
      catch (Exception e)
      {
        AnsiConsole.WriteException(e);
      }
    }
}
