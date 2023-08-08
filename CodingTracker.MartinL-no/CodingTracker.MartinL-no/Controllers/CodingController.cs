using CodingTracker.MartinL_no.DAL;

namespace CodingTracker.MartinL_no.Controllers;

internal class CodingController
{
    private readonly ICodingSessionRepository _sessionRepository;

	public CodingController(ICodingSessionRepository sessionRepository)
	{
        _sessionRepository = sessionRepository;
    }

}
