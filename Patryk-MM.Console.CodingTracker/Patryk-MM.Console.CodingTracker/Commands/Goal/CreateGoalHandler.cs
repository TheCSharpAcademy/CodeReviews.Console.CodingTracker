using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Services;
using Spectre.Console;

namespace Patryk_MM.Console.CodingTracker.Commands.Goal {
    /// <summary>
    /// Handles the creation of coding goals.
    /// </summary>
    public class CreateGoalHandler {
        private readonly TrackerService _trackerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateGoalHandler"/> class with the specified <paramref name="trackerService"/>.
        /// </summary>
        /// <param name="trackerService">The TrackerService instance to be used for handling goals.</param>
        public CreateGoalHandler(TrackerService trackerService) {
            _trackerService = trackerService;
        }

        /// <summary>
        /// Handles the creation of a coding goal.
        /// </summary>
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
