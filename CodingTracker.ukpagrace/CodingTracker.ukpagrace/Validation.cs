using Spectre.Console;
using System.Globalization;

namespace CodingTracker.ukpagrace
{
    internal class Validation
    {
        public int ValidateGoal() {
            return AnsiConsole.Prompt(
                new TextPrompt<int>("[blue]Set a coding goal for this month, goal must be an integer[/]")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]That's not a valid goal[/]")
                    .Validate(goal =>
                    {
                        return goal switch
                        {
                            <= 0 => ValidationResult.Error("[red]You coding hours must be greater 1[/]"),
                            _ => ValidationResult.Success(),
                        };
                    })
            );
        }

        public bool ValidateRange(string firstRangeString, string secondRangeString, string filterFormat)
        {
            DateTime.TryParseExact( firstRangeString, filterFormat, CultureInfo.InvariantCulture,System.Globalization.DateTimeStyles.None,out DateTime firstRange);
            DateTime.TryParseExact(secondRangeString, filterFormat, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime secondRange);
            return firstRange > secondRange;
        }
    }
}
