using Patryk_MM.Console.CodingTracker.Models;
using Spectre.Console;

namespace Patryk_MM.Console.CodingTracker.Utilities {
    public static class UserInput {
        public static DateTime GetDate(string prompt) {
            DateTime date = new DateTime();
            string dateInput = AnsiConsole.Prompt(
                    new TextPrompt<string>(prompt)
                    .Validate(input => {
                        if (input == "exit") return ValidationResult.Success();
                        else if (Validation.ValidateDateInput(input, out date)) {
                            return ValidationResult.Success();
                        } else {
                            return ValidationResult.Error("Invalid date format. Please enter a valid date.");
                        }
                    }));
            return dateInput == "exit" ? DateTime.MinValue : date;
        }

        public static int GetSessionId(List<CodingSession> sessions) {
            int sessionId = AnsiConsole.Prompt(
                new TextPrompt<int>("Please provide an Id of the coding session or type '0' (zero) to cancel: ")
                .Validate(id => {
                    if (id < 0) return ValidationResult.Error("Id must be a non-negative number.");
                    else if (id == 0) return ValidationResult.Success();
                    else if (id > sessions.Count) return ValidationResult.Error("Provided Id not found.");
                    else return ValidationResult.Success();
                }
                ));
            return sessionId;
        }
        public static bool ConfirmAction(string description) {
            var result = AnsiConsole.Confirm(description);

            return result;

        }
    }
}
