using Spectre.Console;
using System.Globalization;

internal class SessionDate
{
    internal static DateTime GetDate()
    {
        string message = $"Enter session's date in \"{Config.DateFormat}\" format or press 'Enter' key for today's date: ";
        AnsiConsole.Markup(message);
        string? dateInput = Console.ReadLine();

        while (true)
        {
            dateInput ??= "";
            bool isValid = DateTime.TryParseExact(dateInput, Config.DateFormat,
                new CultureInfo("en-US"), DateTimeStyles.None, out DateTime date);

            if (dateInput.Trim() == "")
            {
                dateInput = DateTime.Today.ToString(Config.DateFormat);
            }
            else if (isValid && date > DateTime.Today)
            {
                AnsiConsole.MarkupLine("[red]Entered date can not exceed today's date, retry.[/]");
                AnsiConsole.Markup($"{message}");
                dateInput = Console.ReadLine();
            }
            else if (!isValid)
            {
                AnsiConsole.Markup($"[red]Invalid input, retry.[/]\n{message}");
                dateInput = Console.ReadLine();
            }
            else
            {
                AnsiConsole.MarkupLine($"Date: [yellow]{date.ToString(Config.DateFormat)}[/]");
                return date;
            }
        }
    }
}
