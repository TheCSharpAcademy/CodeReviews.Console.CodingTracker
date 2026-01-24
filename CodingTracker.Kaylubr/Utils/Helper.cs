using System.Globalization;
using Spectre.Console;

namespace CodingTracker.Utils;

internal static class Helper
{
    internal static DateTime GetTime(string message)
    {
        while (true)
        {
            var time = AnsiConsole.Ask<string>($"Enter [green]{message}[/] session time in the format (dd-MM-yy HH-mm): ");

            if (DateTime.TryParseExact(time, "dd-MM-yy HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                return DateTime.ParseExact(time, "dd-MM-yy HH:mm", new CultureInfo("en-US"));
            }

            AnsiConsole.MarkupLine("[red]Invalid Format or Invalid Date & Time[/]");
        }

    }

    internal static TimeSpan GetDuration(DateTime start, DateTime end)
    {
        return end - start;
    }

    internal static bool ValidateTime(DateTime start, DateTime end)
    {
        if (start > end)
        {
            AnsiConsole.MarkupLine("[red]Starting time shouldn't be greater than the end time.[/]");
            return false;
        }

        return true;
    }

    internal static void PrintSuccessOperation()
    {
        AnsiConsole.MarkupLine("\n[green]Successful operation![/]");

        AnsiConsole.WriteLine("\nPress any key to continue..");
        Console.ReadKey();
    }
}
