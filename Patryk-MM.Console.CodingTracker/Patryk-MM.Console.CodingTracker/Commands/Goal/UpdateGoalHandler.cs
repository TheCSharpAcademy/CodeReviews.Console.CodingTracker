using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Services;
using Spectre.Console;

namespace Patryk_MM.Console.CodingTracker.Commands.Goal {
    /// <summary>
    /// Handles updating a coding goal.
    /// </summary>
    public class UpdateGoalHandler {
        private readonly TrackerService _trackerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateGoalHandler"/> class with the specified <paramref name="trackerService"/>.
        /// </summary>
        /// <param name="trackerService">The TrackerService instance to be used for updating goals.</param>
        public UpdateGoalHandler(TrackerService trackerService) {
            _trackerService = trackerService;
        }

        /// <summary>
        /// Handles updating a coding goal.
        /// </summary>
        /// <param name="goal">The coding goal to be updated.</param>
        public void Handle(CodingGoal goal) {
            int hours = AnsiConsole.Prompt(
                new TextPrompt<int>("Please provide a new number of hours for the monthly goal: ")
                .Validate(h => {
                    if (h < 0) return ValidationResult.Error("Number of hours must be a positive number.");
                    else return ValidationResult.Success();
                }));

            goal.Hours = hours;

            _trackerService.UpdateGoal(goal);
            AnsiConsole.MarkupLine("[green]Goal successfully updated![/]");
        }
    }
}
