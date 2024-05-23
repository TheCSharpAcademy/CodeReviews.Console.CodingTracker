using CodingTracker.Models;
using CodingTracker.IDataRepository;
using CodingTracker.Utilities;

namespace CodingTracker.Services
{
    public class CodingSessionService : ICodingSessionService
    {
        private readonly ICodingSessionRepository _sessionRepository;
        private readonly IUtils _utils;

        public CodingSessionService(ICodingSessionRepository sessionRepository, IUtils utils)
        {
            _sessionRepository = sessionRepository;
            _utils = utils;
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
            var listOfTimes = _utils.ValidatedTimes();
            session.StartTime = listOfTimes[0];
            session.EndTime = listOfTimes[1];
            session.Duration = _utils.GetSessionDuration(session.StartTime, session.EndTime);

            _sessionRepository.InsertSessionToDatabase(session.StartTime, session.EndTime, session.Duration);
        }

        public void UpdateSession()
        {
            throw new NotImplementedException();
        }
    }
}