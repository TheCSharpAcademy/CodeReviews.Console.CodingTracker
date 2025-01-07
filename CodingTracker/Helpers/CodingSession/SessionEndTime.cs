using Spectre.Console;
using System.Globalization;

internal class SessionEndTime
{
    internal static DateTime GetEndTime(DateTime date, DateTime startTime)
    {
        string message = (date != DateTime.Today) ?
            $"Enter session end time (\"{Config.TimeFormat}\" format): " :
            $"Enter session end time (\"{Config.TimeFormat}\" format) " +
            $"or press 'Enter' key for current time: ";
        AnsiConsole.Markup(message);
        string? timeInput = Console.ReadLine();
        DateTime endTime;

        while (true)
        {
            timeInput ??= "";
            bool isValid = DateTime.TryParseExact(timeInput, Config.TimeFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out _);

            if (timeInput.Trim() == "" && date == DateTime.Today)
            {
                timeInput = DateTime.Now.ToString(Config.TimeFormat);
                endTime = DateTimeHelper.ConcatenateDateAndTime(date, timeInput);
                if (endTime == startTime)
                {
                    AnsiConsole.MarkupLine($"[red]End session time can not be the same as the start time.[/]");
                    AnsiConsole.Markup($"{message}");
                    timeInput = Console.ReadLine();
                }
                else return endTime;
            }
            else if (isValid)
            {
                endTime = DateTimeHelper.ConcatenateDateAndTime(date, timeInput);

                if (endTime == startTime)
                {
                    AnsiConsole.MarkupLine($"[red]End session time can not be the same as the start time.[/]");
                    AnsiConsole.Markup($"{message}");
                    timeInput = Console.ReadLine();
                }
                else if (endTime < startTime)
                {
                    AnsiConsole.Markup($"[red]End session time can not be earlier than start time.[/]\n" +
                        $"{message}");
                    timeInput = Console.ReadLine();
                }
                else if (endTime > DateTime.Now)
                {
                    AnsiConsole.Markup($"[red]End session time can not exceed current time.[/]\n" +
                        $"{message}");
                    timeInput = Console.ReadLine();
                }
                else return endTime;
            }
            else
            {
                AnsiConsole.Markup($"[red]Invalid input, retry.[/]\n{message}");
                timeInput = Console.ReadLine();
            }
        }
    }
}
