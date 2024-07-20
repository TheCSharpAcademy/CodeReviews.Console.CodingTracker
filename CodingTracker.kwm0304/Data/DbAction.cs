using System.Configuration;
using System.Data;
using System.Data.SQLite;
using CodingTracker.kwm0304.Enums;
using CodingTracker.kwm0304.Models;
using Dapper;
using Spectre.Console;

namespace CodingTracker.kwm0304.Data;

public partial class DbAction
{
  private readonly string _connString;
  private readonly string _dbPath;

  public DbAction()
  {
    _connString = ConfigurationManager.ConnectionStrings["CodingTrackerDb"].ConnectionString;
    _dbPath = ConfigurationManager.AppSettings["DbFilePath"]!;
  }
  //ON START
  public void CreateDatabaseIfNotExists()
  {
    if (!File.Exists(_dbPath))
    {
      using (File.Create(_dbPath))
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
    const string queryString = @"CREATE TABLE IF NOT EXISTS CodingSessions (
    Id INTEGER PRIMARY KEY,
    StartTime TEXT NOT NULL,
    EndTime TEXT NOT NULL,
    SessionLength INTEGER NOT NULL
);";
    const string checkTableQuery = @"SELECT name FROM sqlite_master WHERE type='table' AND name='CodingSessions';";
    try
    {
      connection.Execute(queryString);
      var tableName = connection.QueryFirstOrDefault<string>(checkTableQuery);
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
    (@StartTime, @EndTime, @SessionLength);";

    try
    {
      DateTime start = session.StartTime;
      DateTime end = session.EndTime;
      TimeSpan length = session.SessionLength;
      connection.Execute(queryString, new
      {
        StartTime = Utils.Validator.ConvertDateTimeToString(start),
        EndTime = Utils.Validator.ConvertDateTimeToString(end),
        SessionLength = Utils.Validator.ConvertTimeToInt(length)
      });
      AnsiConsole.MarkupLine("[green]Session added successfully[/]");
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
  //GET ALL
  public List<CodingSession> GetAllSessions()
  {
    using var connection = new SQLiteConnection(_connString);
    connection.Open();

    const string queryString = @"SELECT Id, StartTime, EndTime, SessionLength FROM CodingSessions";

    var dbSessions = connection.Query(queryString);

    var sessions = dbSessions.Select(s => new CodingSession
    {
      Id = Convert.ToInt32(s.Id),
      StartTime = Utils.Validator.ConvertTextToDateTime(s.StartTime),
      EndTime = Utils.Validator.ConvertTextToDateTime(s.EndTime),
      SessionLength = TimeSpan.FromSeconds(Convert.ToInt64(s.SessionLength))
    }).ToList();
    return sessions;
  }
  //GET BY DATE
  public List<CodingSession> GetSessionsByDateRange(DateRange range)
  {
    using IDbConnection connection = new SQLiteConnection(_connString);
    DateTime today = DateTime.Now;
    int days = Utils.Validator.ToDays(range);
    try
    {
      var sessions = GetAllSessions();
      DateTime beginningAt = today.AddDays(-days);
      var inRange = sessions.Where(s => s.StartTime >= beginningAt && s.EndTime <= today).ToList();
      AnsiConsole.WriteLine(inRange.Count + "*****************************");
      return inRange;
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
}