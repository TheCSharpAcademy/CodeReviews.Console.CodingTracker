using CodingTrackerLibrary;
using System.Globalization;

namespace CodingTracker.CoreyJordan;
internal static class UserInput
{
    // General short form date format - "g"
    private const string dateForm = "(mm/dd/yyyy hh:mm AM/PM)";

    internal static string GetString()
    {
        string input = Console.ReadLine()!;
        return input;
    }

    internal static DateTime GetDate(Session part)
    {
        ConsoleDisplay display = new();
        DateTime date;

        Console.Clear();
        Console.Write($"Enter {part} date {dateForm}: ");
        string input = Console.ReadLine()!;

        while (!DateTime.TryParseExact(input, "g", new CultureInfo("en-US"), DateTimeStyles.None, out date))
        {
            display.InvalidInput(input);
            Console.Write($"Enter {part} date {dateForm}: ");
            input = Console.ReadLine()!;
        }

        return date;
    }
}
