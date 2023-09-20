using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodingTracker.Mo3ses.Interface;
using CodingTracker.Mo3ses.Models;

namespace CodingTracker.Mo3ses.Controller
{
    public class CodingSessionController
    {
        private readonly ICodingSessionRepository _codingSessionRepo;

        public CodingSessionController(ICodingSessionRepository codingSessionRepo)
        {
            _codingSessionRepo = codingSessionRepo;
        }

        public void Create(string startTime, string endTime)
        {
            DateTime startTimeParsed = DateTime.Parse(startTime);
            DateTime endTimeParsed = DateTime.Parse(endTime);

            CodingSession codingSession = new CodingSession();
            codingSession.StartTime = startTimeParsed;
            codingSession.EndTime = endTimeParsed;

            _codingSessionRepo.Create(codingSession);
        }

        public void Delete(int codingSessionId)
        {
            CodingSession codingSession = new();
            codingSession.Id = codingSessionId;

            _codingSessionRepo.Delete(codingSession);
        }

        public List<CodingSession> GetAll()
        {
            return _codingSessionRepo.GetAll();
        }

        public void Update(int id, string startTime, string endTime)
        {
            DateTime startTimeParsed = DateTime.Parse(startTime);
            DateTime endTimeParsed = DateTime.Parse(endTime);

            CodingSession codingSession = new CodingSession();
            codingSession.StartTime = startTimeParsed;
            codingSession.EndTime = endTimeParsed;
            codingSession.Id = id;

            _codingSessionRepo.Update(codingSession);
        }
    }
}