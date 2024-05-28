using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Services;
using Patryk_MM.Console.CodingTracker.Utilities;
using Spectre.Console;

namespace Patryk_MM.Console.CodingTracker.Commands {
    public class CreateSessionHandler {
        private readonly TrackerService _trackerService;

        public CreateSessionHandler(TrackerService trackerService) {
            _trackerService = trackerService;
        }

        public void Handle(List<CodingSession> sessions) {
            CodingSession newSession = new CodingSession();
            do {
                newSession.StartDate = UserInput.GetDate();
                newSession.EndDate = UserInput.GetDate();

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

                // If both validations pass, create the session and exit the loop
                break;

            } while (true); // Loop until a valid session is provided


            _trackerService.CreateSession(newSession);
        }

    }
}
