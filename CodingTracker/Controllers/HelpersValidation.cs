using System.Globalization;
using Spectre.Console;

namespace CodingTracker.Controllers;

public class HelpersValidation
{
    internal class InputZero : Exception
    {
    }

    internal static string DateInputValidation(string input, string message)
    {
        while (!DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            AnsiConsole.Markup($"[bold red]Invalid date format.[/]\n");
            input = UserInput.GetDateInput();
        }

        return input;
    }

    internal static string TimeInputValidation(string input, string message)
    {
        while (!DateTime.TryParseExact(input, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            AnsiConsole.Markup($"[bold red]Invalid date format.[/]\n");
            input = UserInput.GetDateInput();
        }

        return input;
    }

    internal static string GetDateTimeInput() => $"{UserInput.GetDateInput()} {UserInput.GetTimeInput()}";
}