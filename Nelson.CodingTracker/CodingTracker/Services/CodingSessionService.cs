using CodingTracker.Models;
using CodingTracker.IDataRepository;

namespace CodingTracker.Services
{
    public class CodingSessionService : ICodingSessionService
    {
        private readonly ICodingSessionRepository _sessionRepository;

        public CodingSessionService(ICodingSessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public void DeleteSession()
        {
            throw new NotImplementedException();
        }

        public void GetAllSessions()
        {
            _sessionRepository.GetFromDatabase();
        }

        public void InsertSession(CodingSession session)
        {
            throw new NotImplementedException();
        }

        public void UpdateSession()
        {
            throw new NotImplementedException();
        }
    }
}