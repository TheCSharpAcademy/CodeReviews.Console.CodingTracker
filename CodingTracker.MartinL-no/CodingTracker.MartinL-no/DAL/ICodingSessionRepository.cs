using CodingTracker.MartinL_no.Models;

namespace CodingTracker.MartinL_no.DAL;

internal interface ICodingSessionRepository
{
    public List<CodingSession> GetCodingSessions();

    public CodingSession GetCodingSession(int id);

    public List<CodingSession> GetCodingSessionFromDate(DateTime fromDate);

    public bool InsertCodingSession(CodingSession codingSession);

    public bool DeleteCodingSession(int id);

    public bool UpdateCodingSession(CodingSession codingSession);
}
