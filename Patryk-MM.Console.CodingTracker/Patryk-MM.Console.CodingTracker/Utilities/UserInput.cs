using Spectre.Console;

namespace Patryk_MM.Console.CodingTracker.Utilities {
    public static class UserInput {
        public static DateTime GetDate() {
            DateTime date = new DateTime();
            string dateInput = AnsiConsole.Prompt(
                    new TextPrompt<string>("Please provide a date: ")
                    .Validate(input => {
                        if(Validation.ValidateDateInput(input, out date)) {
                            return ValidationResult.Success();
                        } else {
                            return ValidationResult.Error("Invalid date format. Please enter a valid date.");
                        }
                    }));
            return date;
        }
    }
}
