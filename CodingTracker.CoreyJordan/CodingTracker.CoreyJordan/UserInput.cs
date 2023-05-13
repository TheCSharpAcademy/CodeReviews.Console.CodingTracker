using CodingTrackerLibrary;
using System.Globalization;

namespace CodingTracker.CoreyJordan;
internal static class UserInput
{
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
        Console.Write($"Enter {part} date (mm/dd/yy): ");
        string input = Console.ReadLine()!;

        while (!DateTime.TryParseExact(input, "MM/dd/yy", new CultureInfo("en-US"), DateTimeStyles.None, out date))
        {
            display.InvalidInput(input);
            Console.Write($"Enter {part} date (mm/dd/yy): ");
            input = Console.ReadLine()!;
        }

        return date;
    }
}
