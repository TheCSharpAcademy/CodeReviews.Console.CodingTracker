using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Services;

namespace Patryk_MM.Console.CodingTracker.Queries.Session
{
    public class GetSessionFromListHandler
    {
        private static TrackerService _trackerService;

        public GetSessionFromListHandler(TrackerService trackerService)
        {
            _trackerService = trackerService;
        }

        public CodingSession Handle()
        {
            return _trackerService.GetSessionFromList();
        }
    }
}
