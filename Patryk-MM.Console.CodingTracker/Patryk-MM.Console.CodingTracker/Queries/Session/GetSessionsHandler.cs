using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Services;

namespace Patryk_MM.Console.CodingTracker.Queries.Session {
    public class GetSessionsHandler {

        private readonly TrackerService _trackerService;

        public GetSessionsHandler(TrackerService trackerService) {
            _trackerService = trackerService;
        }
        public List<CodingSession> Handle() {
            return _trackerService.GetSessions().OrderBy(s => s.StartDate).ToList();
        }
    }
}