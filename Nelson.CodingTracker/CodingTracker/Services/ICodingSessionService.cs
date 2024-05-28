using CodingTracker.Models;

namespace CodingTracker.Services
{
    public interface ICodingSessionService
    {
        void GetAllSessions();
        void InsertSession(CodingSession session);
        void UpdateSession();
        void DeleteSession();
        void StartCodingSession();
        void StopCodingSession();
        void CodingSessionsByPeriod();
    }
}