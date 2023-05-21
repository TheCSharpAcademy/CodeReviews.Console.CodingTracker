using CodingTrackerLibrary;
using System.Globalization;

namespace CodingTracker.CoreyJordan;
internal static class UserInput
{    internal static string GetString()
    {
        string input = Console.ReadLine()!;
        return input;
    }

    internal static DateTime GetDate(Session part)
    {
        const string dateForm = "(mm/dd/yyyy hh:mm AM/PM)";
        ConsoleDisplay display = new();
        DateTime date;

        Console.Clear();
        Console.Write($"Enter {part} date {dateForm} or \"now\" to enter current time: ");
        string input = Console.ReadLine()!;

        if (input.ToLower() == "now")
        {
            return DateTime.Now;
        }

        while (!DateTime.TryParseExact(input, "g", new CultureInfo("en-US"), DateTimeStyles.None, out date))
        {
            display.InvalidInput(input);
            Console.Write($"Enter {part} date {dateForm}: ");
            input = Console.ReadLine()!;
        }

        return date;
    }

    internal static int GetInteger(string prompt)
    {
        ConsoleDisplay display = new();
        int output;

        Console.Write(prompt);
        string input = Console.ReadLine()!;

        while (!int.TryParse(input, out output))
        {
            display.InvalidInput(input);
            Console.Write(prompt);
            input = Console.ReadLine()!;
        }

        return output;
    }
}
