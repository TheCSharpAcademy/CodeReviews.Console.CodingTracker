using CodingTracker.Dejmenek.Models;

namespace CodingTracker.Dejmenek.DataAccess.Interfaces
{
    public interface ICodingSessionRepository
    {
        void AddCodingSession(string startDate, string endDate, int duration);
        void DeleteCodingSession(int id);
        List<CodingSession> GetAllCodingSessions();
        IEnumerable<(string year, int durationSum)> GetYearlyCodingSessionReport();
        IEnumerable<(string year, string month, int durationSum)> GetMonthlyCodingSessionReport();
    }
}
