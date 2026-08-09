using Spectre.Console;

namespace CyrillParfen.CodingTracker.Helpers;

internal class InputValidation
{
    internal static int GetNumberInput(string prompt)
    {
        AnsiConsole.MarkupLine($"[yellow]{prompt}[/]");
        string userInput = Console.ReadLine();
        int validNumber;

        while (!IsInputValid(userInput, out validNumber))
        {
            AnsiConsole.MarkupLine("[red]Invalid number.Try again.[/]");
            userInput = Console.ReadLine();
        }

        return validNumber;
    }

    internal static bool IsInputValid(string input, out int number)
    {
        return int.TryParse(input, out number) && number >= 0;
    }
}
