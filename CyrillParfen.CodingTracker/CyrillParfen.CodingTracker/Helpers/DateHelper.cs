using CyrillParfen.CodingTracker.Exceptions;
using CyrillParfen.CodingTracker.Model;
using Spectre.Console;
using System.Globalization;

namespace CyrillParfen.CodingTracker.Helpers;

internal class DateHelper
{
    internal static bool IsStartBeforeEnd(DateTime start, DateTime end)
    {
        return end > start;
    }

    internal static bool IsSessionMissing(CodingSession codingSession, int id)
    {
        if (codingSession is null)
        {
            AnsiConsole.MarkupLine($"[red]No record found with ID:{id}[/]");
            AnsiConsole.MarkupLine($"[blink]Press any key to continue...[/]");
            Console.ReadKey();
            return true;
        }

        return false;
    }

    internal static DateTime GetValidDate(string prompt)
    {
        DateTime inputDate;

        const string dateFormat = "dd.MM.yyyy HH:mm";
        var culture = new CultureInfo("en-US");

        AnsiConsole.MarkupLine($"[yellow]{prompt} (Format: {dateFormat}) (e.g. 31.12.1999 23:59) or type 0 to return to main menu.[/]");
        string userInput = Console.ReadLine();

        while (!IsValidDate(userInput, dateFormat, out inputDate))
        {
            if (userInput == "0") throw new ReturnToMainMenuException();

            AnsiConsole.MarkupLine($"[red]Invalid date. Format: {dateFormat}. (31.12.1999 23:59)" +
                $" or type 0 to return to Main Menu or try again...[/]");

            userInput = Console.ReadLine();
        }

        return inputDate;
    }

    internal static bool IsValidDate(string userInput, string dateFormat, out DateTime inputDate)
    {
        var culture = new CultureInfo("en-US");

        return DateTime.TryParseExact(userInput, dateFormat, culture, DateTimeStyles.None, out inputDate);
    }
}
