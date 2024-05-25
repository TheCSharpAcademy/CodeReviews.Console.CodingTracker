using CodingTracker.Models;

namespace CodingTracker.IDataRepository
{
    public interface ICodingSessionRepository
    {
        void GetFromDatabase();
        void InsertSessionToDatabase(DateTime startTime, DateTime endTime, string duration);
    }
}