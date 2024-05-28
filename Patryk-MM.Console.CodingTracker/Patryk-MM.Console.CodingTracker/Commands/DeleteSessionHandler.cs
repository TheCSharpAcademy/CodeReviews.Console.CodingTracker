using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patryk_MM.Console.CodingTracker.Commands {
    public class DeleteSessionHandler {
        private readonly TrackerService _trackerService;

        public DeleteSessionHandler(TrackerService trackerService) {
            _trackerService = trackerService;
        }

        public void Handle(CodingSession session) {
            _trackerService.DeleteSession(session);
        }
    }
}
