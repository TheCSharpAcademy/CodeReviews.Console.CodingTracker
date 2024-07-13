using CodingTracker.kwm0304.Models;
using CodingTracker.kwm0304.Repositories;
using CodingTracker.kwm0304.Utils;
using CodingTracker.kwm0304.Views;
using Spectre.Console;

namespace CodingTracker.kwm0304.Services;

public class CodingSessionService
{
  private readonly CodingSessionRepository _repository;
  public CodingSessionService()
  {
    _repository = new CodingSessionRepository();
  }

  //GET 
  public CodingSession? GetCodingSession(int id)
  {
    try
    {
      if (Validator.IsIdValid(id))
      {
        return _repository.GetCodingSessionById(id);
      }
      else
      {
        AnsiConsole.WriteLine("No session matches this id");
        return null;
      }
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
      return null;
    }
  }

  public List<CodingSession>? GetAllSessions()
  {
    List<CodingSession>? allSessions = _repository.GetAllCodingSessions();
    if (Validator.IsListValid(allSessions))
    {
      return allSessions;
    }
    else
    {
      AnsiConsole.WriteLine("Unable to get sessions");
      return null;
    }
  }
  //POST
  //Called when session is complete
  public void CreateSession(CodingSession session)
  {
    try
    {
      _repository.CreateCodingSession(session);
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
    }
  }

  public void EditSession(int minAdjustment, int id)
  {
    try
    {
      if (Validator.IsIdValid(id))
        _repository.UpdateCodingSessionById(id, minAdjustment);
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
    }
  }
  public void DeleteSessionById(int id)
  {
    try
    {
      if (Validator.IsIdValid(id))
        _repository.DeleteCodingSessionById(id);
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
      _repository.DeleteAllSessions();
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
    }
  }
}
