using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Services;
using Patryk_MM.Console.CodingTracker.Utilities;
using Spectre.Console;

namespace Patryk_MM.Console.CodingTracker.Commands.Session {
    public class UpdateSessionHandler {
        private readonly TrackerService _trackerService;

        public UpdateSessionHandler(TrackerService trackerService) {
            _trackerService = trackerService;
        }

        public void Handle(CodingSession newSession, List<CodingSession> sessions) {
            do {
                newSession.StartDate = UserInput.GetDate("Please provide a new starting date of a new session using format \"dd.MM.yyyy hh:mm:ss\" \nor type 'exit' to cancel: ");
                if (newSession.StartDate == DateTime.MinValue) break;
                newSession.EndDate = UserInput.GetDate("Please provide a new ending date of a new session using format \"dd.MM.yyyy hh:mm:ss\" \nor type 'exit' to cancel: ");
                if (newSession.EndDate == DateTime.MinValue) break;

                // Validate date order
                if (!Validation.ValidateDateOrder(newSession.StartDate, newSession.EndDate)) {
                    AnsiConsole.MarkupLine("[red]Error: The end date must be after the start date. Please try again.[/]");
                    continue; // Continue to the next iteration of the loop
                }

                // Validate session overlap
                if (Validation.ValidateSessionOverlap(sessions, newSession)) {
                    AnsiConsole.MarkupLine("[red]Error: New session overlaps with an existing one. Please try again.[/]");
                    continue; // Continue to the next iteration of the loop
                }

                if (Validation.ValidateFutureDate(newSession.StartDate) || Validation.ValidateFutureDate(newSession.EndDate)) {
                    AnsiConsole.MarkupLine("[red]Error: Dates must not be in the future. Please try again.[/]");
                    continue; // Continue to the next iteration of the loop
                }

                // If all validations pass, create the session and exit the loop
                break;

            } while (true); // Loop until a valid session is provided

            if (newSession.StartDate == DateTime.MinValue || newSession.EndDate == DateTime.MinValue) {
                AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
                return;
            }

            if (UserInput.ConfirmAction("Are you sure to update this session?")) {
                _trackerService.UpdateSession(newSession);
                AnsiConsole.MarkupLine("[green]Session updated![/]");
            } else {
                AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
            }

        }
    }
}
