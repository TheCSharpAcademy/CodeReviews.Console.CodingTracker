using Spectre.Console;
using System.Globalization;

internal class SessionStartTime
{
    internal static DateTime GetStartTime(DateTime date)
    {
        string message = $"Enter session start time in \"{Config.TimeFormat}\" format: ";
        AnsiConsole.Markup(message);
        string? timeInput = Console.ReadLine();

        while (true)
        {
            timeInput ??= "";
            bool isValid = DateTime.TryParseExact(timeInput, Config.TimeFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime startTime);

            if (date == DateTime.Today && startTime > DateTime.Now)
            {
                AnsiConsole.MarkupLine("[red]Entered time can not exceed current time, retry.[/]");
                AnsiConsole.Markup($"Enter valid start time: ");
                timeInput = Console.ReadLine();
            }
            else if (!isValid)
            {
                AnsiConsole.Markup($"[red]Invalid input, retry.[/]\n{message}");
                timeInput = Console.ReadLine();
            }
            else return DateTimeHelper.ConcatenateDateAndTime(date, timeInput);
        }
    }
}
