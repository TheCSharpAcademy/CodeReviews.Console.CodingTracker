using CodingTracker.Models;
using CodingTracker.IDataRepository;
using CodingTracker.Utilities;
using CodingTracker.ConsoleInteraction;
using System.Diagnostics;

namespace CodingTracker.Services
{
    public class CodingSessionService : ICodingSessionService
    {
        private readonly ICodingSessionRepository _sessionRepository;
        private readonly IUserInteraction _userInteraction;
        private readonly IUtils _utils;
        private Stopwatch _stopwatch;

        public CodingSessionService(ICodingSessionRepository sessionRepository, IUtils utils, IUserInteraction userInteraction)
        {
            _sessionRepository = sessionRepository;
            _userInteraction = userInteraction;
            _utils = utils;
            _stopwatch = new Stopwatch();
        }

        public void DeleteSession()
        {
            int id = _utils.ConvertToInt(_userInteraction.GetUserInput());

            if (id == 0) return;

            _sessionRepository.DeleteSessionFromDatabase(id);
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
            int id = _utils.ConvertToInt(_userInteraction.GetUserInput());

            if (id == 0) return;

            _userInteraction.ShowMessage("\n[Yellow]Which property would you like to update[/]");
            _userInteraction.ShowMessage("\n[DarkGreen]1. Start Time[/]");
            _userInteraction.ShowMessage("\n[DarkGreen]2. End Time[/]");
            _userInteraction.ShowMessage("\n[Green]Input: [/]");

            switch (_userInteraction.GetUserInput())
            {
                case "1":
                    DateTime startTime = _utils.ValidatedStartTime();
                    _sessionRepository.UpdateStartTimeInDatabase(id, startTime);
                    break;
                case "2":
                    DateTime endTime = _utils.ValidatedEndTime();
                    _sessionRepository.UpdateEndTimeInDatabase(id, endTime);
                    break;
                default:
                    _userInteraction.ShowMessageTimeout("\n[Red]You have inputed a wrong choice. Going back to Menu[/]\n");
                    break;
            }
        }

        public void StartCodingSession()
        {
            _stopwatch.Start();
            _userInteraction.ShowMessageTimeout("\n[Green]Starting Coding Session...[/]\n");
            _userInteraction.ShowMessageTimeout("\n[Red]Press any key to stop session...[/]\n");
            Console.ReadKey();
            StopCodingSession();
        }

        public void StopCodingSession()
        {
            _stopwatch.Stop();

            var session = new CodingSession
            {
                StartTime = DateTime.Now.Subtract(_stopwatch.Elapsed),
                EndTime = DateTime.Now,
                Duration = _stopwatch.Elapsed.ToString(),
            };

            _sessionRepository.InsertSessionToDatabase(session.StartTime, session.EndTime, session.Duration);
            _userInteraction.ShowMessageTimeout("\n[Green]Session Stopped.[/]\n");
            _userInteraction.ShowMessageTimeout($"\n[Green]Start Time: { session.StartTime }[/]\n");
            _userInteraction.ShowMessageTimeout($"\n[Green]End Time: { session.EndTime }[/]\n");
            _userInteraction.ShowMessageTimeout($"\n[Green]Duration: { session.Duration }[/]\n");
        }
    }
}