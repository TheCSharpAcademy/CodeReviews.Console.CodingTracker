using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Services;

namespace Patryk_MM.Console.CodingTracker.Queries.Session {
    /// <summary>
    /// Handles the retrieval of a single coding session from the list of sessions.
    /// </summary>
    public class GetSessionFromListHandler {
        private static TrackerService _trackerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSessionFromListHandler"/> class.
        /// </summary>
        /// <param name="trackerService">The service used to manage coding sessions.</param>
        public GetSessionFromListHandler(TrackerService trackerService) {
            _trackerService = trackerService;
        }

        /// <summary>
        /// Retrieves a single coding session from the list of sessions.
        /// </summary>
        /// <returns>The selected <see cref="CodingSession"/>.</returns>
        public CodingSession Handle() {
            return _trackerService.GetSessionFromList();
        }
    }
}
