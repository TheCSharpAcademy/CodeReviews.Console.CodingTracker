using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Services;

namespace Patryk_MM.Console.CodingTracker.Commands {
    public class UpdateSessionHandler {
        private readonly TrackerService _trackerService;

        public UpdateSessionHandler(TrackerService trackerService) {
            _trackerService = trackerService;
        }

        public void Handle(CodingSession session) {
            _trackerService.UpdateSession(session);
        }
    }
}
