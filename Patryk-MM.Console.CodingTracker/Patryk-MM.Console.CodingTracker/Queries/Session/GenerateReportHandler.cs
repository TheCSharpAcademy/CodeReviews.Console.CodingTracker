using Patryk_MM.Console.CodingTracker.Services;
using Patryk_MM.Console.CodingTracker.Utilities;
using Spectre.Console;

namespace Patryk_MM.Console.CodingTracker.Queries.Session {
    public class GenerateReportHandler {
        private readonly TrackerService _trackerService;

        public GenerateReportHandler(TrackerService trackerService) {
            _trackerService = trackerService;
        }

        public void Handle() {
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();

            do {
                startDate = UserInput.GetDate("Please provide a starting date of a report using format \"dd.MM.yyyy hh:mm:ss\" \nor type 'exit' to cancel: ");
                if (startDate == DateTime.MinValue) break;
                endDate = UserInput.GetDate("Please provide an ending date of a report using format \"dd.MM.yyyy hh:mm:ss\" \nor type 'exit' to cancel: ");
                if (endDate == DateTime.MinValue) break;

                // Validate date order
                if (!Validation.ValidateDateOrder(startDate, endDate)) {
                    AnsiConsole.MarkupLine("[red]Error: The end date must be after the start date. Please try again.[/]");
                    continue; // Continue to the next iteration of the loop
                }


                if (Validation.ValidateFutureDate(startDate) || Validation.ValidateFutureDate(endDate)) {
                    AnsiConsole.MarkupLine("[red]Error: Dates must not be in the future. Please try again.[/]");
                    continue; // Continue to the next iteration of the loop
                }

                // If all validations pass, create the session and exit the loop
                break;

            } while (true); // Loop until a valid session is provided


            if (startDate == DateTime.MinValue || endDate == DateTime.MinValue) {
                AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
                return;
            } else {
                var getSessionsHandler = new GetSessionsHandler(_trackerService);
                var sessions = getSessionsHandler.Handle();

                sessions = sessions.Where(s => s.StartDate >= startDate && s.EndDate <= endDate).ToList();
                if (sessions.Count > 0) {
                    // Print sessions
                    DataVisualization.PrintSessions(sessions);

                    // Calculate total duration
                    TimeSpan totalDuration = sessions.Aggregate(TimeSpan.Zero, (total, session) => total + session.Duration);

                    // Calculate average duration
                    TimeSpan averageDuration = sessions.Count > 0 ? TimeSpan.FromSeconds((int)(totalDuration.TotalSeconds / sessions.Count)) : TimeSpan.Zero;

                    // Print total and average duration
                    AnsiConsole.MarkupLine($"Total duration: [cyan]{totalDuration}[/]");
                    AnsiConsole.MarkupLine($"Average duration: [cyan]{averageDuration}[/]");
                } else {
                    AnsiConsole.WriteLine("No sessions in specified period.");
                }
            }
        }
    }
}

