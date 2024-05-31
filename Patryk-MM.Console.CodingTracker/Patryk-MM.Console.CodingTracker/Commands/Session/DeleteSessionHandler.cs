using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Services;
using Patryk_MM.Console.CodingTracker.Utilities;
using Spectre.Console;

namespace Patryk_MM.Console.CodingTracker.Commands.Session {
    /// <summary>
    /// Handles deleting a coding session.
    /// </summary>
    public class DeleteSessionHandler {
        private readonly TrackerService _trackerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteSessionHandler"/> class with the specified <paramref name="trackerService"/>.
        /// </summary>
        /// <param name="trackerService">The TrackerService instance to be used for deleting sessions.</param>
        public DeleteSessionHandler(TrackerService trackerService) {
            _trackerService = trackerService;
        }

        /// <summary>
        /// Handles deleting a coding session.
        /// </summary>
        /// <param name="session">The coding session to be deleted.</param>
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
