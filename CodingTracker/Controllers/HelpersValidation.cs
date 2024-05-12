using System.Globalization;
using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker.Controllers;

public static class HelpersValidation
{
    internal static DateTime ConvertToTime(string datetimeString)
    {
        DateTime convertedString = default;
        try
        {
            convertedString = DateTime.ParseExact(datetimeString, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        }
        catch (FormatException)
        {
        }

        return convertedString;
    }
    
    internal static string DateInputValidation(string input, string message, string dateInput, string inputType)
    {
        while (!DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            AnsiConsole.Markup($"[bold red]Invalid date format.[/]\n");
            input = UserInput.GetDateInput(dateInput, inputType);
        }

        return input;
    }

    internal static string TimeInputValidation(string input, string message, string timeInput)
    {
        while (!DateTime.TryParseExact(input, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            AnsiConsole.Markup($"[bold red]Invalid time format.[/]\n");
            input = UserInput.GetTimeInput(timeInput);
        }

        return input;
    }
    
    internal static int NumberInputValidation(string input, string message)
    {
        int numberInput;
        while (!int.TryParse(input, out numberInput) || numberInput < 0)
        {
            AnsiConsole.WriteLine($"[bold red]{input} is not a valid number format. Please provide an integer only.[/]\n");
            input = AnsiConsole.Ask<string>(message);
        }

        return numberInput;
    }

    private static string GetDateTimeInput(string startOrEnd, string inputType)
    {
        return $"{UserInput.GetDateInput(startOrEnd, inputType)} {UserInput.GetTimeInput(startOrEnd)}";
    }

    internal static CodingSession GetSessionData()
    {
        string startDateTime = "";
        string endDateTime = "";
        try
        {
            startDateTime = GetDateTimeInput("start", "coding session");
            endDateTime = GetDateTimeInput("end", "coding session");

            while (ConvertToTime(endDateTime) < ConvertToTime(startDateTime))
            {
                AnsiConsole.Markup(
                    $"[bold red]{endDateTime} is before {startDateTime}. Please provide a correct end date & time.[/]\n\n");
                endDateTime = GetDateTimeInput("end", "coding session");
            }
        }
        catch (InputZero)
        {
            Console.WriteLine("Returning to main menu...");
        }

        return new CodingSession("", startDateTime, endDateTime);
    }
    internal static TimeSpan TotalTime(List<CodingSession> tableData)
    {
        TimeSpan totalTime = default;
        foreach (var row in tableData)
        {
            totalTime += TimeSpan.Parse(row.Duration);
        }

        return totalTime;
    }
    
    internal class InputZero : Exception
    {
    }
}

