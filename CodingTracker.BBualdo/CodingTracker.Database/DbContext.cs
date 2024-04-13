using CodingTracker.Database.enums;
using CodingTracker.Database.Helpers;
using CodingTracker.Database.Models;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace CodingTracker.Database.DbContext;

public class DbContext
{
  private readonly string _connectionString;
  private readonly SessionDataAccess _sessionDataAccess;
  private readonly GoalsDataAccess _goalsDataAccess;
  public GoalTracker GoalTracker { get; set; }

  public DbContext()
  {
    _connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString")!;
    CreateTables();
    SeedData();
    _sessionDataAccess = new SessionDataAccess(_connectionString);
    _goalsDataAccess = new GoalsDataAccess(_connectionString);
    GoalTracker = new GoalTracker();
  }

  public bool UpdateGoals()
  {
    List<CodingSession> sessions = _sessionDataAccess.GetAllSessions();
    List<Goal> goals = _goalsDataAccess.GetAllGoals(false);
    List<Goal> incompletedGoals = goals.Where(goal => !Convert.ToBoolean(goal.Is_Completed)).ToList();

    foreach (Goal goal in incompletedGoals)
    {
      int isCompleted = GoalTracker.IsGoalCompleted(goal, sessions);
      _goalsDataAccess.UpdateGoal(goal, isCompleted);
    }

    return true;
  }

  public bool AddGoal()
  {
    string startDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
    string endDate = UserInput.GetGoalEndDate(startDate);
    int targetDuration = UserInput.GetTargetDuration(startDate, endDate);

    _goalsDataAccess.AddGoal(startDate, endDate, targetDuration);
    return true;
  }

  public bool DeleteGoal()
  {
    _goalsDataAccess.DeleteGoal();
    return true;
  }

  public bool GetAllGoals(bool filterByCompleted = false)
  {
    List<Goal> goals = _goalsDataAccess.GetAllGoals(filterByCompleted);

    ConsoleEngine.GetGoalsTable(goals);

    AnsiConsole.WriteLine("Press any key to return to Main Menu.");
    Console.ReadKey();
    return true;
  }

  public bool GetReport(ReportOptions reportOption, OrderOptions? orderOption)
  {
    List<CodingSession> sessions = _sessionDataAccess.GetAllSessions(reportOption);
    List<CodingSession> orderedSessions;
    if (orderOption == OrderOptions.ASC)
    {
      orderedSessions = sessions.OrderBy(session => session.Duration).ToList();
    }
    else if (orderOption == OrderOptions.DESC)
    {
      orderedSessions = sessions.OrderByDescending(session => session.Duration).ToList();
    }
    else
    {
      orderedSessions = sessions;
    }

    ConsoleEngine.GetCodingSessionsTable(orderedSessions);

    ConsoleEngine.GetDurationSummary(orderedSessions, reportOption);

    AnsiConsole.WriteLine("Press any key to return to Main Menu.");
    Console.ReadKey();
    return true;
  }

  public bool DeleteSession()
  {
    _sessionDataAccess.DeleteSession();
    return true;
  }

  public bool UpdateSession()
  {
    _sessionDataAccess.UpdateSession();
    return true;
  }

  public bool InsertSession()
  {
    UpdateGoals();
    string startDate = UserInput.GetStartDate();
    string endDate = UserInput.GetEndDate(startDate);
    _sessionDataAccess.InsertSession(startDate, endDate);
    return true;
  }

  public bool InsertSession(string startDate, string endDate)
  {
    UpdateGoals();
    _sessionDataAccess.InsertSession(startDate, endDate);
    return true;
  }

  public bool GetAllSessions()
  {
    List<CodingSession> sessions = _sessionDataAccess.GetAllSessions();

    ConsoleEngine.GetCodingSessionsTable(sessions);

    AnsiConsole.WriteLine("Press any key to return to Main Menu.");
    Console.ReadKey();
    return true;
  }

  private void CreateTables()
  {
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      string createTablesSql = @"CREATE TABLE IF NOT EXISTS sessions(
                              session_id INTEGER PRIMARY KEY AUTOINCREMENT,
                              start_date TEXT,
                              end_date TEXT,
                              duration INT);
                              CREATE TABLE IF NOT EXISTS goals(
                              goal_id INTEGER PRIMARY KEY AUTOINCREMENT,
                              start_date TEXT,
                              end_date TEXT,
                              target_duration INTEGER,
                              is_completed INTEGER)";

      using (SqliteCommand createCommand = new SqliteCommand(createTablesSql, connection))
      {
        createCommand.ExecuteNonQuery();
      }
    }
  }

  private void SeedData()
  {
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      string countSessionsSql = "SELECT COUNT(*) FROM sessions";

      Random random = new Random();

      using (SqliteCommand countCommand = new SqliteCommand(countSessionsSql, connection))
      {
        int recordsNumber = Convert.ToInt32(countCommand.ExecuteScalar());

        if (recordsNumber == 0)
        {
          Console.WriteLine("Loading sessions...");

          for (int i = 0; i < 10; i++)
          {
            DateTime startDateTime = DateTime.Now.AddDays(-random.Next(0, 365)).AddHours(random.Next(0, 24)).AddMinutes(random.Next(0, 60));
            DateTime endDateTime = startDateTime.AddHours(random.Next(0, 7)).AddMinutes(random.Next(0, 60));

            TimeSpan durationTimeSpan = endDateTime - startDateTime;

            string startDate = startDateTime.ToString("yyyy-MM-dd HH:mm");
            string endDate = endDateTime.ToString("yyyy-MM-dd HH:mm");
            int duration = Convert.ToInt32(durationTimeSpan.TotalMinutes);

            string insertSql = $"INSERT INTO sessions(start_date, end_date, duration) VALUES('{startDate}', '{endDate}', {duration})";

            using (SqliteCommand insertCommand = new SqliteCommand(insertSql, connection))
            {
              insertCommand.ExecuteNonQuery();
            }
          }
        }
      }
    }

    Console.Clear();
  }
}