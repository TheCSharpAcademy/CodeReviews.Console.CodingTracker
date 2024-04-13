using CodingTracker.Database.Helpers;
using CodingTracker.Database.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace CodingTracker.Database;

public class GoalsDataAccess
{
  private readonly string _connectionString;

  public GoalsDataAccess(string connectionString)
  {
    _connectionString = connectionString;
  }

  public bool UpdateGoal(Goal goal, int isCompleted)
  {
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      string selectSql = $"SELECT EXISTS(SELECT 1 FROM goals WHERE goal_id={goal.Goal_Id})";

      using (SqliteCommand selectCommand = new SqliteCommand(selectSql, connection))
      {
        if (Convert.ToInt32(selectCommand.ExecuteScalar()) == 0)
        {
          return false;
        }
      }

      string updateSql = $"UPDATE goals SET is_completed={isCompleted} WHERE goal_id={goal.Goal_Id}";

      using (SqliteCommand updateCommand = new SqliteCommand(updateSql, connection))
      {
        updateCommand.ExecuteNonQuery();
      }
    }

    return true;
  }

  public List<Goal> GetAllGoals(bool filterByCompleted)
  {
    List<Goal> goals = new List<Goal>();

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      string selectSql;

      if (!filterByCompleted)
      {
        selectSql = "SELECT * FROM goals";
      }
      else
      {
        selectSql = "SELECT * FROM goals WHERE is_completed=1";
      }

      goals = connection.Query<Goal>(selectSql).ToList();
    }

    return goals;
  }

  public bool DeleteGoal()
  {
    List<Goal> goals = GetAllGoals(false);
    ConsoleEngine.GetGoalsTable(goals);

    int id = AnsiConsole.Ask<int>("Type [green]ID[/] of the goal you want to delete: ");

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      string selectSql = $"SELECT EXISTS(SELECT 1 FROM goals WHERE goal_id={id})";

      using (SqliteCommand selectCommand = new SqliteCommand(selectSql, connection))
      {
        if (Convert.ToInt32(selectCommand.ExecuteScalar()) == 0)
        {
          AnsiConsole.Markup("[red]Goal with given id doesn't exists.[/] Press any key to return to Main Menu.");
          Console.ReadKey();
          return false;
        }
      }

      string deleteSql = $"DELETE FROM goals WHERE goal_id={id}";

      using (SqliteCommand updateCommand = new SqliteCommand(deleteSql, connection))
      {
        updateCommand.ExecuteNonQuery();
      }
    }

    AnsiConsole.Markup("[green]Deleting Completed.[/] Press any key to return to Main Menu.");
    Console.ReadKey();
    return true;

  }

  public bool AddGoal(string startDate, string endDate, int targetDuration)
  {
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      string insertSql = $"INSERT INTO goals(start_date, end_date, target_duration, is_completed) VALUES('{startDate}', '{endDate}', {targetDuration}, 0)";

      using (SqliteCommand insertCommand = new SqliteCommand(insertSql, connection))
      {
        insertCommand.ExecuteNonQuery();
      }
    }

    AnsiConsole.Markup($"[green]Inserting completed![/] Good luck! Press any key to return to Main Menu");
    Console.ReadKey();
    return true;
  }
}