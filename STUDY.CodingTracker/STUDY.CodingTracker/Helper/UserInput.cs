using Spectre.Console;

namespace STUDY.CodingTracker.Helper;

internal static class UserInput
{
    public static string GetUserDateInput(string period)
    {
        string dateInput = AnsiConsole.Ask<string>($"Please enter the {period} date (format: dd/MM/yyyy HH:mm) (Enter -1 to return to main menu):");
        return dateInput;
    }

    public static string GetUserIDInput(string operation)
    {
        string idInput = AnsiConsole.Ask<string>($"Please enter the id of the coding session you want to {operation} (Enter -1 to return to main menu):");

        return idInput;
    }

    public static string GetUserFilterPeriod(FilterChoice filterChoice)
    {
        Console.Clear();

        string periodInput = AnsiConsole.Ask<string>($"Please enter a date included in the {filterChoice} you've chosen to filter the coding sessions displayed. (Format: dd/MM/yyyy) (Enter -1 to return to the main menu):");

        return periodInput;
    }
}
