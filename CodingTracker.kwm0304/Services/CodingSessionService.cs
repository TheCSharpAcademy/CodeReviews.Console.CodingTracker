using CodingTracker.kwm0304.Enums;
using CodingTracker.kwm0304.Models;
using CodingTracker.kwm0304.Repositories;

public class CodingSessionService
{
  private readonly CodingSessionRepository _sessionRepository;

  public CodingSessionService()
  {
    _sessionRepository = new CodingSessionRepository();
  }

  public List<CodingSession> GetAllCodingSessions()
  {
    return _sessionRepository.GetAllCodingSessions()!;
  }

  public void CreateCodingSession(CodingSession session)
  {
    _sessionRepository.CreateCodingSession(session);
  }

  public bool UpdateCodingSessionById(CodingSession session, int change, string type)
  {
    DateTime newEnd = session.EndTime;
    TimeSpan newSessionLength = session.SessionLength;

    switch (type)
    {
      case "Seconds":
        newEnd = session.EndTime.AddSeconds(change);
        newSessionLength = session.SessionLength.Add(TimeSpan.FromSeconds(change));
        break;
      case "Minutes":
        newEnd = session.EndTime.AddMinutes(change);
        newSessionLength = session.SessionLength.Add(TimeSpan.FromMinutes(change));
        break;
      case "Hours":
        newEnd = session.EndTime.AddHours(change);
        newSessionLength = session.SessionLength.Add(TimeSpan.FromHours(change));
        break;
      default:
        throw new ArgumentException("Invalid time type");
    }

    if (newEnd > DateTime.Now)
    {
      return false;
    }
    else
    {
      session.EndTime = newEnd;
      session.SessionLength = newSessionLength;
      int sessionLengthInSeconds = CodingTracker.kwm0304.Utils.Validator.ConvertTimeToInt(newSessionLength);
      _sessionRepository.UpdateCodingSession(session.Id, newEnd, sessionLengthInSeconds);
      return true;
    }
  }

  public List<CodingSession> GetAllCodingSessionsInDateRange(DateRange range)
  {
    return _sessionRepository.GetAllCodingSessionsInDateRange(range)!;
  }

  public void DeleteCodingSession(CodingSession session)
  {
    if (session != null)
    {
      _sessionRepository.DeleteCodingSession(session);
    }
  }
}
