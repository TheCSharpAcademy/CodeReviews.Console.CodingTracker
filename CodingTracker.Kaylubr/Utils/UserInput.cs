using Spectre.Console;
using System.Globalization;

namespace CodingTracker.Utils;

internal static class UserInput
{
    internal static int GetID(string mode)
    {
        AnsiConsole.WriteLine();
        return AnsiConsole.Ask<int>($"\nEnter the [green]ID[/] of the row to be [bold]{mode}[/]: ");
    }

    internal static (string StartTime, string EndTime) GetStartAndEndTime()
    {
        DateTime start;
        DateTime end;

        do
        {
            AnsiConsole.WriteLine();
            start = GetTime("START");
            end = GetTime("END");
        } while (!Helper.ValidateTime(start, end));

        string startTime = start.ToString();
        string endTime = end.ToString();

        return (startTime, endTime);
    }

    static DateTime GetTime(string message)
    {
        while (true)
        {
            var time = AnsiConsole.Ask<string>($"Enter [bold green]{message}[/] session time in the format (dd-MM-yy HH-mm): ");

            if (DateTime.TryParseExact(time, "dd-MM-yy HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                return DateTime.ParseExact(time, "dd-MM-yy HH:mm", new CultureInfo("en-US"));
            }

            AnsiConsole.MarkupLine("[red]Invalid Format or Invalid Date & Time[/]");
        }

    }

    internal static string GetDuration(string start, string end)
    {
        DateTime startTime = DateTime.Parse(start);
        DateTime endTime = DateTime.Parse(end);

        return (endTime - startTime).ToString(@"hh\:mm\:ss");
    }

}