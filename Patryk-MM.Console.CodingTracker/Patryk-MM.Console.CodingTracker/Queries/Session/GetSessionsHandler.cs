using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Services;
using System.Collections.Generic;

namespace Patryk_MM.Console.CodingTracker.Queries.Session {
    /// <summary>
    /// Handles retrieving coding sessions from the database.
    /// </summary>
    public class GetSessionsHandler {

        private readonly TrackerService _trackerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSessionsHandler"/> class with the specified <paramref name="trackerService"/>.
        /// </summary>
        /// <param name="trackerService">The TrackerService instance to be used for retrieving sessions.</param>
        public GetSessionsHandler(TrackerService trackerService) {
            _trackerService = trackerService;
        }

        /// <summary>
        /// Retrieves coding sessions from the database and orders them by start date.
        /// </summary>
        /// <returns>A list of CodingSession objects representing the retrieved sessions.</returns>
        public List<CodingSession> Handle() {
            return _trackerService.GetSessions().OrderBy(s => s.StartDate).ToList();
        }
    }
}
