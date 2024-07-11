using CodingTracker.kwm0304.Repositories;

namespace CodingTracker.kwm0304.Services;

public class CodingSessionService
{
    private readonly CodingSessionRepository _repository;
    public CodingSessionService()
    {
      _repository = new CodingSessionRepository();
    }
}
