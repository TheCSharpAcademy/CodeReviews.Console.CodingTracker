using CodingTracker.MartinL_no.DAL;
using CodingTracker.MartinL_no.Models;

namespace CodingTracker.MartinL_no.Controllers;

internal class CodingController
{
    private readonly ICodingSessionRepository _sessionRepository;

	public CodingController(ICodingSessionRepository sessionRepository)
	{
        _sessionRepository = sessionRepository;
    }

    public List<CodingSession>GetCodingSessions()
    {
        return _sessionRepository.GetCodingSessions();
    }

    public bool InsertCodingSession(string startTimeString, string endTimeString)
    {
        var startTime = DateTime.Parse(startTimeString);
        var endTime = DateTime.Parse(endTimeString);

        var codingSession = new CodingSession(startTime, endTime);

        return _sessionRepository.InsertCodingSession(codingSession);
    }

    public bool DeleteCodingSession(int id)
    {
        return _sessionRepository.DeleteCodingSession(id);
    }
}
