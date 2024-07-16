using System.Configuration;
using System.Data;
using System.Data.SQLite;
using CodingTracker.kwm0304.Enums;
using CodingTracker.kwm0304.Models;
using Dapper;
using Spectre.Console;

namespace CodingTracker.kwm0304.Data;

public class DbAction
{
  private readonly string _connString;
  private readonly string dbPath = ConfigurationManager.AppSettings["DbFilePath"] ?? string.Empty;

  public DbAction()
  {
    _connString = ConfigurationManager.ConnectionStrings["CodingTrackerDb"].ConnectionString;
  }
  //ON START
  public void CreateDatabaseIfNotExists()
  {
    if (!File.Exists(dbPath))
    {
      using (File.Create(dbPath))
      {
        //File created, don't take any more action
      }
    }
    CreateGoalTableIfNotExists();
    CreateHabitTableIfNotExists();
  }
  public void CreateHabitTableIfNotExists()
  {
    using IDbConnection connection = new SQLiteConnection(_connString);

    const string queryString = @"CREATE TABLE IF NOT EXISTS CodingSessions(
    Id INTEGER PRIMARY KEY,
    StartTime TEXT NOT NULL,
    EndTime TEXT NOT NULL,
    SessionLength TEXT NOT NULL
    );";
    try
    {
      connection.Execute(queryString);
    }
    catch (SQLiteException e)
    {
      AnsiConsole.WriteException(e);
    }
  }
  public void CreateGoalTableIfNotExists()
  {
    using IDbConnection connection = new SQLiteConnection(_connString);

    const string queryString = @"CREATE TABLE IF NOT EXISTS Goals(
    GoalId INTEGER PRIMARY KEY,
    GoalName TEXT NOT NULL,
    TargetNumber INTEGER NOT NULL,
    Progress INTEGER NOT NULL,
    AccomplishBy TEXT NOT NULL,
    CreatedOn TEXT NOT NULL,
    EndDate TEXT NOT NULL,
    Accomplished INTEGER DEFAULT 1
    );";
    try
    {
      connection.Execute(queryString);
    }
    catch (SQLiteException e)
    {
      AnsiConsole.WriteException(e);
    }
  }
  //POST
  public void InsertSession(CodingSession session)
  {
    using IDbConnection connection = new SQLiteConnection(_connString);

    const string queryString = @"INSERT INTO CodingSessions
    (StartTime, EndTime, SessionLength)
    VALUES
    (@startTime, @endTime, @sessionLength);";
    try
    {
      connection.Execute(queryString, new
      {
        StartTime = session.StartTime.ToString("O"),
        EndTime = session.EndTime.ToString("O"),
        SessionLength = session.SessionLength.ToString(@"hh\:mm\:ss")
      });
      AnsiConsole.WriteLine("Session added successfully");
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
    (GoalName, TargetNumber, Progress, CreatedOn, EndDate, Accomplished)
    VALUES
    (@goalName, @targetNumber, @progress,  @createdOn, @endDate, @accomplished);";
    try
    {
      connection.Execute(queryString, new
      {
        goal.GoalName,
        goal.TargetNumber,
        goal.Progress,
        CreatedOn = goal.CreatedOn.ToString("O"),
        EndDate = goal.EndDate.ToString("O"),
        Accomplished = goal.Accomplished ? 0 : 1
      });
      AnsiConsole.WriteLine("Goal added successfully");
    }
    catch (SQLiteException e)
    {
      AnsiConsole.WriteException(e);
    }
  }

  //GET ONE
  public CodingSession? GetSessionById(int id)
  {
    using IDbConnection connection = new SQLiteConnection(_connString);
    const string queryString = "SELECT * FROM CodingSessions WHERE Id = @sessionId";
    try
    {
      var session = connection.QueryFirstOrDefault<CodingSession>(queryString, new { sessionId = id });
      return session ?? default;
    }
    catch (SQLiteException e)
    {
      AnsiConsole.WriteException(e);
      return default;
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

  //GET ALL
  public List<CodingSession>? GetAllSessions()
  {
    using IDbConnection connection = new SQLiteConnection(_connString);
    const string queryString = "SELECT * FROM CodingSessions";
    try
    {
      return connection.Query<CodingSession>(queryString).ToList();
    }
    catch (SQLiteException e)
    {
      AnsiConsole.WriteException(e);
      return null;
    }
  }

  public List<Goal>? GetAllGoals()
  {
    using IDbConnection connection = new SQLiteConnection(_connString);
    const string queryString = "SELECT * FROM Goals";
    try
    {
      return connection.Query<Goal>(queryString).ToList();
    }
    catch (SQLiteException e)
    {
      AnsiConsole.WriteException(e);
      return null;
    }
  }

  //GET BY DATE
  public List<CodingSession> GetSessionsByDateRange(DateRange range)
  {
    using IDbConnection connection = new SQLiteConnection(_connString);
    DateTime today = DateTime.Now;
    DateTime begenningAt = DateTime.MinValue;
    switch (range)
    {
      case DateRange.Week:
        begenningAt = today.AddDays(-7);
        break;
      case DateRange.Month:
        begenningAt = today.AddMonths(-1);
        break;
      case DateRange.Year:
        begenningAt = today.AddYears(-1);
        break;
      default:
        break;
    }
    string rangeStr = begenningAt.ToString("O");
    string todayStr = today.ToString("O");
    const string queryString = @"
    SELECT * FROM CodingSessions
    WHERE StartTime >= @begenningAt AND StartTime <= @today;";
    try
    {
      var sessions = connection.Query<CodingSession>(queryString, new { begenningAt = range, today = todayStr }).ToList();
      return sessions;
    }
    catch (SQLiteException e)
    {
      AnsiConsole.WriteException(e);
      return [];
    }
  }
  //EDIT
  public void UpdateSession(int id, int newTime)
  {
    using IDbConnection connection = new SQLiteConnection(_connString);
    const string queryString = "UPDATE CodingSessions SET EndTime = @newTime WHERE Id = @id";
    try
    {
      //??
      connection.Execute(queryString, new { newTime, id });
      AnsiConsole.WriteLine("Session updated successfully");
    }
    catch (SQLiteException e)
    {
      AnsiConsole.WriteException(e);
    }
  }
  public void UpdateGoalProgress(int id, int update)
  {
    using IDbConnection connection = new SQLiteConnection(_connString);
    const string queryString = "UPDATE Goals SET Progress = Progress + @update WHERE GoalId = @id";
    try
    {
      connection.Execute(queryString, new { id, update });
      AnsiConsole.WriteLine("Progress updated successfully");
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
  //DELETE
  public void DeleteSession(int id)
  {
    using IDbConnection connection = new SQLiteConnection(_connString);
    const string queryString = "DELETE FROM CodingSessions WHERE Id = @id";
    try
    {
      connection.Execute(queryString, new { id });
      AnsiConsole.WriteLine("Session Deleted successfully");
    }
    catch (SQLiteException e)
    {
      AnsiConsole.WriteException(e);
    }
  }
  public void DeleteAllSessions()
  {
    using IDbConnection connection = new SQLiteConnection(_connString);
    const string queryString = "DELETE * FROM CodingSessions";
    try
    {
      connection.Execute(queryString);
      AnsiConsole.WriteLine("All sessions deleted");
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

  //(GoalName, TargetNumber, Progress, CreatedOn, EndDate, Accomplished)
  public List<Goal> GetActiveGoals()
  {
    using IDbConnection connection = new SQLiteConnection(_connString);
    string todayStr = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
    const string queryString = @"
                SELECT GoalId, GoalName, TargetNumber, Progress, CreatedOn, EndDate, Accomplished
                FROM Goals
                WHERE EndDate > @Today";
    try
    {
      return connection.Query<Goal>(queryString, new { Today = todayStr }).ToList();
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