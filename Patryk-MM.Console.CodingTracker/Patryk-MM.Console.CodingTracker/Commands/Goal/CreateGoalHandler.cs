using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Services;
using Spectre.Console;

namespace Patryk_MM.Console.CodingTracker.Commands.Goal {
    public class CreateGoalHandler {
        private readonly TrackerService _trackerService;

        public CreateGoalHandler(TrackerService trackerService) {
            _trackerService = trackerService;
        }

        public void Handle() {
            int hours = AnsiConsole.Prompt(
                new TextPrompt<int>("Please provide a number of hours for the monthly goal: ")
                .Validate(h => {
                    if (h < 0) return ValidationResult.Error("Number of hours must be a positive number.");
                    else return ValidationResult.Success();
                }));

            CodingGoal goal = new CodingGoal() {
                Hours = hours,
            };

            _trackerService.CreateGoal(goal);
            AnsiConsole.MarkupLine("[green]Goal successfully created![/]");
        }
    }
}
