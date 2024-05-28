using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Services;

namespace Patryk_MM.Console.CodingTracker.Commands {
    public class CreateSessionHandler {
        private readonly TrackerService _trackerService;

        public CreateSessionHandler(TrackerService trackerService) {
            _trackerService = trackerService;
        }

        public void Handle(CodingSession session) {
            _trackerService.CreateSession(session);
        }

    }
}
