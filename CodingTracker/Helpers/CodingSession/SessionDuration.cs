using Spectre.Console;
using System.Globalization;

internal class SessionDuration
{
    internal static TimeSpan GetDuration(DateTime startTime)
    {
        string message = $"Enter coding session time duration in \"{Config.TimeFormat}\" format: ";
        AnsiConsole.Markup(message);
        string? timeInput = Console.ReadLine();

        while (true)
        {
            timeInput ??= "";
            if (TimeSpan.TryParseExact(timeInput, "hh\\:mm", CultureInfo.InvariantCulture,
                out TimeSpan duration))
            {
                if (startTime + duration > DateTime.Now)
                {
                    AnsiConsole.Markup($"[red]Entered duration exceeds current time.[/]\n{message}");
                    timeInput = Console.ReadLine();
                }
                else if (duration == TimeSpan.Zero)
                {
                    AnsiConsole.Markup($"[red]Can not enter a duration of 00:00.[/]\n{message}");
                    timeInput = Console.ReadLine();
                }
                else return duration;
            }
            else
            {
                AnsiConsole.Markup($"[red]Invalid input, retry.[/]\n{message}");
                timeInput = Console.ReadLine();
            }
        }
    }

    internal static string GetTotalSessionDurationInfo(TimeSpan time)
    {
        int totalDays = time.Days;
        int totalHours = time.Hours % 24;
        int totalMinutes = time.Minutes;

        string days = (totalDays == 1) ? "day" : "days";
        string hours = (totalHours == 1) ? "hour" : "hours";
        string minutes = (totalMinutes == 1) ? "minute" : "minutes";

        if (totalDays > 0)
        {
            return $"{totalDays} {days} {totalHours} {hours} {totalMinutes} {minutes}";
        }
        else if (totalHours > 0)
        {
            return (totalMinutes > 0) ?
                $"{totalHours} {hours} {totalMinutes} {minutes}" :
                $"{totalHours} {hours}";
        }
        else return $"{totalMinutes} {minutes}";
    }
}
