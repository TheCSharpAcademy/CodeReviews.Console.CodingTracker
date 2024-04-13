using CodingTracker.Database.enums;
using CodingTracker.Database.Helpers;
using CodingTracker.Database.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace CodingTracker.Database;

public class SessionDataAccess
{
  private readonly string _connectionString;
  public SessionDataAccess(string connectionString)
  {
    _connectionString = connectionString;
  }

  public List<CodingSession> GetAllSessions()
  {
    List<CodingSession> sessions = new List<CodingSession>();

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      string selectSql = "SELECT * FROM sessions";

      sessions = connection.Query<CodingSession>(selectSql).ToList();
    }

    return sessions;
  }

  public List<CodingSession> GetAllSessions(ReportOptions reportOption)
  {
    List<CodingSession> sessions = new List<CodingSession>();
    string selectSql;

    switch (reportOption)
    {
      case ReportOptions.Daily:
        selectSql = "SELECT * FROM sessions WHERE DATE(start_date) = DATE('now')";
        break;
      case ReportOptions.Weekly:
        selectSql = "SELECT * FROM sessions WHERE strftime('%Y-%W', start_date) = strftime('%Y-%W', 'now')";
        break;
      case ReportOptions.Monthly:
        selectSql = "SELECT * FROM sessions WHERE strftime('%Y-%m', start_date) = strftime('%Y-%m', 'now')";
        break;
      case ReportOptions.Yearly:
        selectSql = "SELECT * FROM sessions WHERE strftime('%Y', start_date) = strftime('%Y', 'now')";
        break;
      default:
        selectSql = "SELECT * FROM sessions";
        break;
    }

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      sessions = connection.Query<CodingSession>(selectSql).ToList();
    }

    return sessions;
  }

  public bool InsertSession(string startDate, string endDate)
  {
    int duration = DateTimeHelper.CalculateDuration(startDate, endDate);

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      string insertSql = $"INSERT INTO sessions(start_date, end_date, duration) VALUES('{startDate}', '{endDate}', {duration})";

      using (SqliteCommand insertCommand = new SqliteCommand(insertSql, connection))
      {
        insertCommand.ExecuteNonQuery();
      }
    }

    AnsiConsole.Markup($"[green]Inserting completed![/] You were coding for {duration} minutes. Press any key to return to Main Menu");
    Console.ReadKey();
    return true;
  }

  public bool UpdateSession()
  {
    List<CodingSession> sessions = GetAllSessions();
    ConsoleEngine.GetCodingSessionsTable(sessions);
    int id = AnsiConsole.Ask<int>("Type [green]ID[/] of the session you want to update: ");

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      string selectSql = $"SELECT EXISTS(SELECT 1 FROM sessions WHERE session_id={id})";

      using (SqliteCommand selectCommand = new SqliteCommand(selectSql, connection))
      {
        if (Convert.ToInt32(selectCommand.ExecuteScalar()) == 0)
        {
          AnsiConsole.Markup("[red]Session with given id doesn't exists.[/] Press any key to return to Main Menu.");
          Console.ReadKey();
          return false;
        }
      }

      string startDate = UserInput.GetStartDate();
      string endDate = UserInput.GetEndDate(startDate);
      int duration = DateTimeHelper.CalculateDuration(startDate, endDate);

      string updateSql = $"UPDATE sessions SET start_date='{startDate}', end_date='{endDate}', duration={duration} WHERE session_id={id}";

      using (SqliteCommand updateCommand = new SqliteCommand(updateSql, connection))
      {
        updateCommand.ExecuteNonQuery();
      }
    }

    AnsiConsole.Markup("[green]Update Completed.[/] Press any key to return to Main Menu.");
    Console.ReadKey();
    return true;
  }

  public bool DeleteSession()
  {
    List<CodingSession> sessions = GetAllSessions();
    ConsoleEngine.GetCodingSessionsTable(sessions);
    int id = AnsiConsole.Ask<int>("Type [green]ID[/] of the session you want to delete: ");

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      string selectSql = $"SELECT EXISTS(SELECT 1 FROM sessions WHERE session_id={id})";

      using (SqliteCommand selectCommand = new SqliteCommand(selectSql, connection))
      {
        if (Convert.ToInt32(selectCommand.ExecuteScalar()) == 0)
        {
          AnsiConsole.Markup("[red]Session with given id doesn't exists.[/] Press any key to return to Main Menu.");
          Console.ReadKey();
          return false;
        }
      }

      string deleteSql = $"DELETE FROM sessions WHERE session_id={id}";

      using (SqliteCommand updateCommand = new SqliteCommand(deleteSql, connection))
      {
        updateCommand.ExecuteNonQuery();
      }
    }

    AnsiConsole.Markup("[green]Deleting Completed.[/] Press any key to return to Main Menu.");
    Console.ReadKey();
    return true;
  }
}