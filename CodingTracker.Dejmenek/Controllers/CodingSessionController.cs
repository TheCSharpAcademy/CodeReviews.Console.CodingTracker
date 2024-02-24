using CodingTracker.Dejmenek.DataAccess.Repositories;
using CodingTracker.Dejmenek.Models;
using CodingTracker.Dejmenek.Services;
using Spectre.Console;

namespace CodingTracker.Dejmenek.Controllers
{
    public class CodingSessionController
    {
        private readonly TrackTimeService _trackTimeService;
        private readonly UserInteractionService _userInteractionService;
        private readonly CodingSessionRepository _codingSessionRepository;
        private DateTime _startDateTime;
        private DateTime _endDateTime;

        public CodingSessionController(TrackTimeService trackTimeService, CodingSessionRepository codingSessionRepository, UserInteractionService userInteractionService)
        {
            _trackTimeService = trackTimeService;
            _codingSessionRepository = codingSessionRepository;
            _userInteractionService = userInteractionService;
        }

        public void StartSession(ref bool shouldStop)
        {
            _startDateTime = DateTime.Now;
            _trackTimeService.Run(ref shouldStop);
        }

        public void EndSession()
        {
            _endDateTime = DateTime.Now;

            int duration = _trackTimeService.GetElapsedTimeInMinutes();
            _trackTimeService.Reset();

            _codingSessionRepository.AddCodingSession(_startDateTime.ToString("yyyy-MM-dd HH:mm"), _endDateTime.ToString("yyyy-MM-dd HH:mm"), duration);
        }

        public void DeleteCodingSession()
        {
            int id = _userInteractionService.GetId();

            _codingSessionRepository.DeleteCodingSession(id);
        }

        public void AddCodingSession()
        {
            string startDateTime = _userInteractionService.GetDateTime();
            string endDateTime = _userInteractionService.GetDateTime();

            _startDateTime = DateTime.Parse(startDateTime);
            _endDateTime = DateTime.Parse(endDateTime);

            while (!Validation.IsChronologicalOrder(_startDateTime, _endDateTime))
            {
                AnsiConsole.MarkupLine("The ending time should always be after the starting time. Try again.");
                startDateTime = _userInteractionService.GetDateTime();
                endDateTime = _userInteractionService.GetDateTime();

                _startDateTime = DateTime.Parse(startDateTime);
                _endDateTime = DateTime.Parse(endDateTime);
            }

            int duration = CalculateDuration();

            _codingSessionRepository.AddCodingSession(startDateTime, endDateTime, duration);
        }

        public List<CodingSession> GetAllCodingSessions()
        {
            return _codingSessionRepository.GetAllCodingSessions();
        }

        public IEnumerable<(string year, int durationSum)> GetYearlyCodingSessionReport()
        {
            return _codingSessionRepository.GetYearlyCodingSessionReport();
        }

        public IEnumerable<(string year, string month, int durationSum)> GetMonthlyCodingSessionReport()
        {
            return _codingSessionRepository.GetMonthlyCodingSessionReport();
        }

        private int CalculateDuration()
        {
            TimeSpan ts = _endDateTime - _startDateTime;
            return (int)ts.TotalMinutes;
        }
    }
}
