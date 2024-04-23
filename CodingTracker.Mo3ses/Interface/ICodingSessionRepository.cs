using CodingTracker.Mo3ses.Models;

namespace CodingTracker.Mo3ses.Interface
{
    public interface ICodingSessionRepository
    {
        public List<CodingSession> GetAll();
        public List<CodingSession> GetSessionsPeriods(DateTime dateTime, int order);
        void Create(CodingSession codingSession);
        void Update(CodingSession codingSession);
        void Delete(CodingSession codingSession);

    }
}