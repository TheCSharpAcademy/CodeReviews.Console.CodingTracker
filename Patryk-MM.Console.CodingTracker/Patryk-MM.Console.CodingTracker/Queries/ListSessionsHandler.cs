using Patryk_MM.Console.CodingTracker.Services;

namespace Patryk_MM.Console.CodingTracker.Queries {
    public class ListSessionsHandler {

        private readonly TrackerService _trackerService;

        public ListSessionsHandler(TrackerService trackerService) {
            _trackerService = trackerService;
        }
        public void Handle() {
            var sessions = _trackerService.GetSessions();

            foreach (var item in sessions) {
                System.Console.WriteLine(item);
            }
        }
    }
}
