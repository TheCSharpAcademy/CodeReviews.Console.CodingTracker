using Patryk_MM.Console.CodingTracker.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;

namespace Patryk_MM.Console.CodingTracker.Utilities {
    /// <summary>
    /// Provides utility methods for user input handling.
    /// </summary>
    public static class UserInput {
        /// <summary>
        /// Prompts the user for a date input.
        /// </summary>
        /// <param name="prompt">The prompt message to display.</param>
        /// <returns>The parsed DateTime value based on user input, or DateTime.MinValue if the user cancels.</returns>
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

        /// <summary>
        /// Prompts the user for a session ID input.
        /// </summary>
        /// <param name="sessions">The list of existing coding sessions.</param>
        /// <returns>The session ID provided by the user, or 0 if the user cancels.</returns>
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

        /// <summary>
        /// Prompts the user for confirmation of an action.
        /// </summary>
        /// <param name="description">The description of the action to confirm.</param>
        /// <returns>True if the user confirms the action, otherwise false.</returns>
        public static bool ConfirmAction(string description) {
            var result = AnsiConsole.Confirm(description);
            return result;
        }
    }
}
