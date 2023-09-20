using CodingTracker.Mo3ses.Models;

namespace CodingTracker.Mo3ses.Interface
{
    public interface ICodingSessionRepository
    {
        public List<CodingSession> GetAll();
        void Create(CodingSession codingSession);
        void Update(CodingSession codingSession);
        void Delete(CodingSession codingSession);

    }
}