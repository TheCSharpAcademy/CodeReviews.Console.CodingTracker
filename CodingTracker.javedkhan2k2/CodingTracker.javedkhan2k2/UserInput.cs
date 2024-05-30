
using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker;

internal static class UserInput
{
    internal static string GetDateTimeInput(string message)
    {
        string? userInput = AnsiConsole.Ask<string?>(message);
        while(!Validation.IsValidDateTimeInput(userInput))
        {
            userInput = AnsiConsole.Ask<string?>($"[bold red]Invalid input ({userInput})[/].\n{message}");
        }
        return userInput;
    }

    internal static int GetIntegerValue(string message)
    {
        string? userInput = AnsiConsole.Ask<string?>(message);
        while(userInput == null || !Int32.TryParse(userInput, out _))
        {
            userInput = AnsiConsole.Ask<string?>($"[bold red]Invalid input {userInput}[/]\n{message}");
        }
        return Convert.ToInt32(userInput);
    }

    internal static CodingSessionDto GetNewCodingSession()
    {
        string startDate = GetDateTimeInput("Enter Start date and time in [bold green](yyyy-MM-dd hh:mm:ss)[/] format ");
        string endDate = GetDateTimeInput("Enter End date and time in [bold green](yyyy-MM-dd hh:mm:ss)[/] format ");
        while(!IsValidDateTimeInputs(startDate, endDate))
        {
            Console.WriteLine($"End Date Time {endDate} should be later than Start Date Time {startDate}");
            startDate = GetDateTimeInput("Enter Start date and time in [bold green](yyyy-MM-dd hh:mm:ss)[/] format ");
            endDate = GetDateTimeInput("Enter End date and time in [bold green](yyyy-MM-dd hh:mm:ss)[/] format ");
        }
        long duration = CalculateDuration(startDate, endDate);
        return new CodingSessionDto {StartTime = startDate, EndTime = endDate, Duration = duration};
    }

    private static bool IsValidDateTimeInputs(string startDate, string endDate)
    {
        DateTime startDateTime = DateTime.Parse(startDate);
        DateTime endDateTime = DateTime.Parse(endDate);
        return DateTime.Compare(endDateTime, startDateTime) > 0 ? true : false;
    }

    private static long CalculateDuration(string startDate, string endDate)
    {
        DateTime startDateTime = DateTime.Parse(startDate);
        DateTime endDateTime = DateTime.Parse(endDate);
        return (long)endDateTime.Subtract(startDateTime).Duration().TotalSeconds;
    }
}