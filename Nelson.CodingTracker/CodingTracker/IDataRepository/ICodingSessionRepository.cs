using CodingTracker.Models;

namespace CodingTracker.IDataRepository
{
    public interface ICodingSessionRepository
    {
        void DeleteSessionFromDatabase(int id);
        void GetFromDatabase();
        void InsertSessionToDatabase(DateTime startTime, DateTime endTime, string duration);
        void UpdateEndTimeInDatabase(int id, DateTime endTime);
        void UpdateStartTimeInDatabase(int id, DateTime startTime);
        void GetFromDatabaseOrdered(IEnumerable<CodingSession> list);
        List<CodingSession> GetAllFromDatabase();
    }
}