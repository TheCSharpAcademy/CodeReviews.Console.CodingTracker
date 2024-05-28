using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patryk_MM.Console.CodingTracker.Queries {
    public class GetSessionFromListHandler {
        private static TrackerService _trackerService;

        public GetSessionFromListHandler(TrackerService trackerService)
        {
            _trackerService = trackerService;
        }

        public CodingSession Handle() { 
            return _trackerService.GetSessionFromList();
        }
    }
}
