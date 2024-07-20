using System.Data.SQLite;
using CodingTracker.kwm0304.Data;
using CodingTracker.kwm0304.Models;
using Spectre.Console;

namespace CodingTracker.kwm0304.Repositories;

public class GoalRepository
{
  private readonly DbAction _dbAction;
  public GoalRepository()
  {
    _dbAction = new DbAction();
    _dbAction.CreateDatabaseIfNotExists();
    _dbAction.CreateGoalTableIfNotExists();
    _dbAction.CreateHabitTableIfNotExists();
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
  public void UpdateGoalProgress(int id, int update)
  {
    try
    {
      _dbAction.UpdateGoalProgress(id, update);
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
    }
  }
  public void UpdateGoalCompletion(int id, bool update)
  {
    try
    {
      _dbAction.UpdateGoalCompletion(update, id);
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
    }
  }

  public List<Goal> GetCompletedGoals()
  {
    return _dbAction.GetCompletedGoals();
  }

  public List<Goal>? GetActiveGoals()
  {
    try
    {
      return _dbAction.GetActiveGoals();
    }
    catch (SQLiteException e)
    {
      AnsiConsole.WriteException(e);
      return [];
    }
  }
  public void SaveGoal(Goal goal)
  {
    _dbAction.SaveGoal(goal);
  }
}