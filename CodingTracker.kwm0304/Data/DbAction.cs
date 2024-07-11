using System.Configuration;
using System.Data.SQLite;
using CodingTracker.kwm0304.Models;

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
    using SQLiteConnection connection = new(_connString);
    connection.Open();
    using var command = connection.CreateCommand();
  }
  public void CreateTableIfNotExists()
  {
    using SQLiteConnection connection = new(_connString);
    connection.Open();
    using var command = connection.CreateCommand();
  }
  //POST
  public void InsertSession()
  {
    using SQLiteConnection connection = new(_connString);
    connection.Open();
    using var command = connection.CreateCommand();
  }
  //GET ONE
  public CodingSession GetSessionById(int id)
  {
    CodingSession session;
    using SQLiteConnection connection = new(_connString);
    connection.Open();
    using var command = connection.CreateCommand();
    return session;
  }
  //GET ALL
  public List<CodingSession> GetAllSessions()
  {
    List<CodingSession> allSessions = new();
    using SQLiteConnection connection = new(_connString);
    connection.Open();
    using var command = connection.CreateCommand();
    return allSessions;
  }
  //GET BY DATE
  public List<CodingSession> GetSessionsByDateRange()
  {
    List<CodingSession> selectedSessions = new();
    using SQLiteConnection connection = new(_connString);
    connection.Open();
    using var command = connection.CreateCommand();
    return selectedSessions;
  }
  //EDIT
  public void UpdateSession(int id)
  {
    using SQLiteConnection connection = new(_connString);
    connection.Open();
    using var command = connection.CreateCommand();
  }
  //DELETE
  public void DeleteSession(int id)
  {
    using SQLiteConnection connection = new(_connString);
    connection.Open();
    using var command = connection.CreateCommand();
  }
  public void DeleteAllSessions()
  {
    using SQLiteConnection connection = new(_connString);
    connection.Open();
    using var command = connection.CreateCommand();
  }
}
