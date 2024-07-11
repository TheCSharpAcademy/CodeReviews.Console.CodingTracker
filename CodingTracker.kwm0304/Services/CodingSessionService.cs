using CodingTracker.kwm0304.Models;
using CodingTracker.kwm0304.Repositories;
using CodingTracker.kwm0304.Utils;
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
    if (Validator.IsIdValid(id))
    {
      try
      {
        return _repository.GetCodingSessionById(id);
      }
      catch (Exception e)
      {
        AnsiConsole.WriteException(e);
        return null;
      }
    }
    else
    {
      AnsiConsole.WriteLine("No session matches this id");
      return null;
    }
  }
  public List<CodingSession>? GetAllSessions()
  {
    List<CodingSession>? allSessions = _repository.GelAllCodingSessions();
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
  public void CreateSession(StartSessionDto dto)
  {
    if (Validator.IsValidTime(dto.DtoStartTime))
    {
      DateTime endingTime = DateTime.Now;
      CodingSession session = new(dto.DtoStartTime, endingTime);
      _repository.CreateCodingSession(session);
    }

  }
}
