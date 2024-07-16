using CodingTracker.kwm0304.Enums;
using CodingTracker.kwm0304.Models;
using CodingTracker.kwm0304.Repositories;

namespace CodingTracker.kwm0304.Services;

public class CodingSessionService
{
    private readonly CodingSessionRepository _repository;
    public CodingSessionService()
    {
      _repository = new CodingSessionRepository();
    }
    public void CreateCodingSession(CodingSession session)
    {
      _repository.CreateCodingSession(session);
    }
    public CodingSession GetCodingSessionById(int id)
    {
      return _repository.GetCodingSessionById(id)!;
    }
    public List<CodingSession> GetAllCodingSessions()
    {
      return _repository.GetAllCodingSessions()!;
    }
    public List<CodingSession> GetAllCodingSessionsInDateRange(DateRange range)
    {
      return _repository.GetAllCodingSessionsInDateRange(range)!;
    }
    public void UpdateCodingSessionById(int id, int numMinutes)
    {
      _repository.UpdateCodingSessionById(id, numMinutes);
    }
    public void DeleteCodingSessionById(int id)
    {
      _repository.DeleteCodingSessionById(id);
    }
    public void DeleteAllCodingSessions()
    {
      _repository.DeleteAllSessions();
    }
}
