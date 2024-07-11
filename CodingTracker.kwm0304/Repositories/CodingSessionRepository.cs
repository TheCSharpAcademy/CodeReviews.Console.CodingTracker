using CodingTracker.kwm0304.Data;
using CodingTracker.kwm0304.Models;
using Spectre.Console;

namespace CodingTracker.kwm0304.Repositories;

public class CodingSessionRepository
{
  private readonly DbAction _dbActions;
  public CodingSessionRepository()
  {
    _dbActions = new DbAction();
    _dbActions.CreateDatabaseIfNotExists();
    _dbActions.CreateTableIfNotExists();
  }
  //GET
  public CodingSession? GetCodingSessionById(int id)
  {
    try
    {
      return _dbActions.GetSessionById(id);
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
      return null;
    }
  }
  public List<CodingSession>? GelAllCodingSessions()
  {
    try
    {
      List<CodingSession> sessions = _dbActions.GetAllSessions();
      return sessions;
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
      return null;
    }
  }
  //POST
  public void CreateCodingSession(CodingSession session)
  {
    try
    {
      _dbActions.InsertSession(session);
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
    }
  }
  //UPDATE
  public void UpdateCodingSessionById(int id, string updatedTimeString)
  {
    try
    {
      _dbActions.UpdateSession(id, updatedTimeString);
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
    }
  }
  //DELETE
  public void DeleteCodingSessionById(int id)
  {
    try
    {
      _dbActions.DeleteSession(id);
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
    }
  }
  public void DeleteAllSessions()
  {
    try
    {
      _dbActions.DeleteAllSessions();
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
    }
  }
}
