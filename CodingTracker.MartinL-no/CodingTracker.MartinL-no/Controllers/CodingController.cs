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

    public DateTime StartSession()
    {
        return DateTime.Now;
    }

    public List<CodingSession> GetCodingSessions()
    {
        return _sessionRepository.GetCodingSessions();
    }

    public CodingSession GetCodingSession(int id)
    {
        return _sessionRepository.GetCodingSession(id);
    }

    private List<CodingSession> GetCodingSessionsFromDate(DateTime fromDateTime)
    {
        return _sessionRepository.GetCodingSessionFromDate(fromDateTime);
    }

    public List<CodingSession> GetCodingSessionsByDays(int days)
    {
        var period = new TimeSpan(days, 0, 0, 0);
        var fromDateTime = DateTime.Now.Subtract(period);

        return GetCodingSessionsFromDate(fromDateTime);
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

    public bool UpdateCodingSession(int id, string startTimeString, string endTimeString)
    {
        var codingSession = GetCodingSession(id);
        if (codingSession == null) return false;

        codingSession.StartTime = DateTime.Parse(startTimeString);
        codingSession.EndTime = DateTime.Parse(endTimeString);

        return _sessionRepository.UpdateCodingSession(codingSession);
    }
}
