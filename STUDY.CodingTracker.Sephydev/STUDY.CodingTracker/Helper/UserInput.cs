using Spectre.Console;

namespace STUDY.CodingTracker.Helper;

internal static class UserInput
{
    public static string GetUserDateInput(string period)
    {
        string dateInput = AnsiConsole.Ask<string>($"Please enter the {period} date (format: dd/MM/yyyy HH:mm):");
        return dateInput;
    }

    public static string GetUserIDInput(string operation)
    {
        string idInput = AnsiConsole.Ask<string>($"Please enter the id of the coding session you want to {operation}:");

        return idInput;
    }

    public static string GetUserFilterPeriod(FilterChoice filterChoice)
    {
        Console.Clear();

        string periodInput = AnsiConsole.Ask<string>($"Please enter the {filterChoice} number you want the coding sessions displayed be filtered on:");

        return periodInput;
    }
}
