using CodingTracker.Models;
using Spectre.Console;
using System.Globalization;

namespace CodingTracker.ConsoleApp.Services;

/// <summary>
/// Helper service for getting valid user inputs of different types.
/// </summary>
internal static class UserInputService
{
    internal static DateTime? GetDateTime(string prompt, string format, Func<string, UserInputValidationResult> validate)
    {
        while (true)
        {
            var input = AnsiConsole.Ask<string>(prompt);
            if (input == "0")
            {
                return null;
            }

            var validationResult = validate(input);
            if (validationResult.IsValid)
            {
                return DateTime.ParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None);
            }

            AnsiConsole.WriteLine(validationResult.Message);
        }
    }

    internal static double GetDouble(string prompt, Func<double, UserInputValidationResult> validate)
    {
        while (true)
        {
            var input = AnsiConsole.Ask<double>(prompt);
            var validationResult = validate(input);
            if (validationResult.IsValid)
            {
                return input;
            }

            AnsiConsole.WriteLine(validationResult.Message);
        }
    }

}
