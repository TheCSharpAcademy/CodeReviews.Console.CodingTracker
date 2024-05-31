using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Services;
using Patryk_MM.Console.CodingTracker.Utilities;
using Spectre.Console;

namespace Patryk_MM.Console.CodingTracker.Commands.Session {
    public class DeleteSessionHandler {
        private readonly TrackerService _trackerService;

        public DeleteSessionHandler(TrackerService trackerService) {
            _trackerService = trackerService;
        }

        public void Handle(CodingSession session) {
            if (UserInput.ConfirmAction("Are you sure to delete this session?")) {
                _trackerService.DeleteSession(session);
                AnsiConsole.MarkupLine("[green]Session deleted![/]");
            } else {
                AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
            }
        }
    }
}
