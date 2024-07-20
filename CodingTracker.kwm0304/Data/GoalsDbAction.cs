using System.Data;
using System.Data.SQLite;
using CodingTracker.kwm0304.Models;
using Dapper;
using Spectre.Console;

namespace CodingTracker.kwm0304.Data;

public partial class DbAction
{
  public void CreateGoalTableIfNotExists()
  {
    using IDbConnection connection = new SQLiteConnection(_connString);
    const string queryString = @"CREATE TABLE IF NOT EXISTS Goals(
    GoalId INTEGER PRIMARY KEY,
    GoalName TEXT NOT NULL,
    TargetNumber INTEGER NOT NULL,
    Progress INTEGER NOT NULL,
    CreatedOn TEXT NOT NULL,
    EndDate TEXT NOT NULL,
    AccomplishBy INTEGER NOT NULL,
    Accomplished INTEGER
    );";
    const string checkTableQuery = @"SELECT name FROM sqlite_master WHERE type='table' AND name='Goals';";
    try
    {
      connection.Execute(queryString);
      var tableName = connection.QueryFirstOrDefault<string>(checkTableQuery);
      if (tableName != null)
      {
        AnsiConsole.MarkupLine("[green]Table 'Goals' exists or was created successfully*****************************************.[/]");
      }
      else
      {
        AnsiConsole.MarkupLine("[red]Table 'Goals' does not exist and was not created*****************************************.[/]");
      }
    }
    catch (SQLiteException e)
    {
      AnsiConsole.WriteException(e);
    }
  }

  public void InsertGoal(Goal goal)
  {
    using IDbConnection connection = new SQLiteConnection(_connString);

    const string queryString = @"INSERT INTO Goals
    (GoalName, TargetNumber, Progress, CreatedOn, EndDate, AccomplishBy, Accomplished)
    VALUES
    (@GoalName, @TargetNumber, @Progress, @CreatedOn, @EndDate, @AccomplishBy, @Accomplished);";

    try
    {
      connection.Execute(queryString, new
      {
        goal.GoalName,
        goal.TargetNumber,
        goal.Progress,
        CreatedOn = Utils.Validator.ConvertDateTimeToString(goal.CreatedOn),
        EndDate = Utils.Validator.ConvertDateTimeToString(goal.EndDate),
        AccomplishBy = Utils.Validator.ToDays(goal.AccomplishBy),
        Accomplished = goal.Accomplished == true ? 0 : 1
      });
      AnsiConsole.MarkupLine("[green]Goal added successfully[/]");
    }
    catch (SQLiteException e)
    {
      AnsiConsole.WriteException(e);
    }
  }

  public Goal? GetGoalById(int id)
  {
    using IDbConnection connection = new SQLiteConnection(_connString);
    const string queryString = "SELECT * FROM Goals WHERE GoalId = @id";
    try
    {
      var goal = connection.QueryFirstOrDefault<Goal>(queryString, new { id = id });
      return goal ?? default;
    }
    catch (SQLiteException e)
    {
      AnsiConsole.WriteException(e);
      return default;
    }
  }

  public List<Goal>? GetAllGoals()
  {
    using IDbConnection connection = new SQLiteConnection(_connString);
    const string queryString = @"SELECT GoalId, GoalName, TargetNumber, Progress, CreatedOn, AccomplishBy, Accomplished FROM Goals";
    var dbGoals = connection.Query(queryString).ToList();

    var goals = dbGoals.Select(g => new Goal
    {
      GoalId = Convert.ToInt32(g.GoalId),
      GoalName = g.GoalName,
      TargetNumber = Convert.ToInt32(g.TargetNumber),
      Progress = Convert.ToInt32(g.Progress),
      CreatedOn = Utils.Validator.ConvertTextToDateTime(g.CreatedOn),
      AccomplishBy = Utils.Validator.ToDateRange(Convert.ToInt32(g.AccomplishBy)),
      Accomplished = Convert.ToBoolean(g.Accomplished)
    }).ToList();
    return goals;
  }

  public void UpdateGoalProgress(int id, int update)
  {
    using IDbConnection connection = new SQLiteConnection(_connString);
    const string queryString = "UPDATE Goals SET Progress = Progress + @update WHERE GoalId = @id";
    try
    {
      connection.Execute(queryString, new { id, update });
      AnsiConsole.WriteLine("Progress updated successfully");
      Thread.Sleep(1500);
    }
    catch (SQLiteException e)
    {
      AnsiConsole.WriteException(e);
    }
  }

  public void UpdateGoalCompletion(bool accomplished, int id)
  {
    using IDbConnection connection = new SQLiteConnection(_connString);
    const string queryString = "UPDATE Goals SET Accomplished = @accomplished WHERE GoalId = @id";
    try
    {
      connection.Execute(queryString, new { Accomplished = accomplished.ToString(), id });
      AnsiConsole.WriteLine("Goal status updated");
    }
    catch (SQLiteException e)
    {
      AnsiConsole.WriteException(e);
    }
  }

  public List<Goal> GetCompletedGoals()
  {
    using IDbConnection connection = new SQLiteConnection(_connString);
    const string queryString = "SELECT * FROM Goals WHERE Accomplished = 0";
    try
    {
      return connection.Query<Goal>(queryString).ToList();
    }
    catch (SQLiteException e)
    {
      AnsiConsole.WriteException(e);
      return [];
    }
  }

  public List<Goal>? GetActiveGoals()
  {
    using IDbConnection connection = new SQLiteConnection(_connString);
    try
    {
      var goals = GetAllGoals();
      DateTime today = DateTime.Today.AddDays(1);
      var active = goals!.Where(g =>
      {
        int days = Utils.Validator.ToDays(g.AccomplishBy);
        DateTime endDate = g.CreatedOn.AddDays(days);
        return endDate > today;
      }).ToList();
      return active;
    }
    catch (SQLiteException e)
    {
      AnsiConsole.WriteException(e);
      return [];
    }
  }

  public void SaveGoal(Goal goal)
  {
    using IDbConnection connection = new SQLiteConnection(_connString);
    const string queryString = @"
                UPDATE Goals
                SET Progress = @Progress,
                Accomplished = @Accomplished
                WHERE GoalId = @GoalId";
    try
    {
      connection.Execute(queryString, new
      {
        goal.Progress,
        goal.Accomplished,
        goal.GoalId
      });
      AnsiConsole.WriteLine("Goal updated successfully");
    }
    catch (SQLiteException e)
    {
      AnsiConsole.WriteException(e);
    }
  }
}