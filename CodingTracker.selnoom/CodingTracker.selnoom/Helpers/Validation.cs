using Spectre.Console;

namespace CodingTracker.selnoom.Helpers;

internal static class Validation
{
    internal static int ConvertMenuInputToInt (string input)
    {
        return int.Parse(input.Split('-')[0].Trim());
    }

    internal static string ValidateTimeInput()
    {
        string timeInput = AnsiConsole.Ask<string>("");
        while (!DateTime.TryParseExact(timeInput, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime startTime))
        {
            if (ReturnToMenu(timeInput))
            {
                return "0";
            }

            AnsiConsole.MarkupLine("[bold red]Invalid input. Please try again.[/]\n");
            timeInput = AnsiConsole.Ask<string>("Enter date (yyyy-MM-dd HH:mm):\n\n");
        }
        return timeInput;
    }

    internal static string ValidateEndTimeInput(string startTime)
    {
        string endTime;

        DateTime parsedStartTime = DateTime.ParseExact(
        startTime,
        "yyyy-MM-dd HH:mm",
        null,
        System.Globalization.DateTimeStyles.None);

        while (true)
        {
            endTime = ValidateTimeInput();

            if (ReturnToMenu(endTime))
            {
                return "0";
            }

            DateTime parsedEndTime = DateTime.ParseExact(
            endTime,
            "yyyy-MM-dd HH:mm",
            null,
            System.Globalization.DateTimeStyles.None);

            if (parsedStartTime > parsedEndTime)
            {
                AnsiConsole.MarkupLine("[bold red]The end time cannot be before the start time. Please try again.[/]\n");
            }
            else
            {
                return endTime;
            }
        }
    }

    internal static bool ReturnToMenu(string input)
    {
        return input.Trim() == "0";
    }

    internal static string FormatDuration(TimeSpan duration)
    {
        string formattedDuration;

        
        return formattedDuration = $"{(int)duration.TotalDays} days, {duration.Hours:D2} hours, {duration.Minutes:D2} minutes";
    }

    internal static int FormatInputToInt(string input)
    {
        int formattedInput;

        while(!int.TryParse(input, out formattedInput))
        {
            input = AnsiConsole.Ask<string>(("[bold red]Invalid input. Please try again.[/]\n"));
        }

        return formattedInput;
    }

    internal static bool CheckIfIdExists(List<int> ids, int input)
    {
        return ids.Contains(input);
    }
}
