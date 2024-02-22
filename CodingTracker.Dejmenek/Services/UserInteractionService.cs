using Spectre.Console;

namespace CodingTracker.Dejmenek.Services
{
    public class UserInteractionService
    {
        public int GetId()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<int>("Enter id: ")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]That is not a valid id[/]")
                    .Validate(Validation.IsPositiveNumber)
                );
        }

        public string GetDateTime()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter date time in 'yyyy-MM-dd HH:mm format': ")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]That is not a valid date time format.[/]")
                    .Validate(Validation.IsValidDateTimeFormat)
                );
        }

        public string GetDate()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter date in 'yyyy-MM-dd format': ")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]That is not a valid date format.[/]")
                    .Validate(Validation.IsValidDateFormat)
                );
        }

        public int GetDuration()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<int>("Enter duration in minutes: ")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]That is not a valid duration format.[/]")
                    .Validate(Validation.IsPositiveNumber)
                );
        }
    }
}
