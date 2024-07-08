using CodingTracker.Dates;
using CodingTracker.Model;
using Spectre.Console;

namespace CodingTracker.Input
{
    public static class InputHelper
    {
        public static string GetDateInput(bool start)
        {
            string s = start ? "start" : "end";
            AnsiConsole.MarkupLine($"Please input the [lime]{s} date[/] in the [lime]M/D/YYYY format[/], type [lime]0[/] to quit, or press [lime]enter[/] for the [lime]current date[/]");

            while (true)
            {
                var input = AnsiConsole.Prompt(new TextPrompt<string>("[silver]Date:[/]").AllowEmpty());
                input = input.Trim().ToLower();

                if (input == "0" || input == string.Empty || DateHelper.ValidateDateFormat(input))
                {
                    return input;
                }

                AnsiConsole.MarkupLine("[red3_1]Invalid date[/]");
            }
        }

        public static string GetTimeInput(bool start)
        {
            string s = start ? "start" : "end";
            AnsiConsole.MarkupLine($"Please input the [lime]{s} time[/] in either [lime]hh:mm tt (12-hour time)[/] or [lime]HH:MM (24-hour time)[/] format, type [lime]0[/] to quit, or press [lime]enter[/] for the [lime]current time[/]");

            while (true)
            {
                var input = AnsiConsole.Prompt(new TextPrompt<string>("[silver]Time:[/]").AllowEmpty());
                input = input.Trim().ToLower();

                if (input == "0" || input == string.Empty || DateHelper.ValidateTimeFormat(input))
                {
                    return input;
                }

                AnsiConsole.MarkupLine("[red3_1]Invalid time[/]");
            }
        }

        public static (DateTime, DateTime)? GetAllDateTimes()
        {
            var startDate = GetDateInput(true);
            if (startDate == "0") return null;
            else if (startDate == "") startDate = DateHelper.GetCurrentDate();

            var startTime = GetTimeInput(true);
            if (startTime == "0") return null;
            else if (startTime == "") startTime = DateHelper.GetCurrentTime();

            var endDate = GetDateInput(false);
            if (endDate == "0") return null;
            else if (endDate == "") endDate = DateHelper.GetCurrentDate();

            var endTime = GetTimeInput(false);
            if (endTime == "0") return null;
            else if (endTime == "") endTime = DateHelper.GetCurrentTime();

            var date1 = DateTime.Parse($"{startDate} {startTime}");
            var date2 = DateTime.Parse($"{endDate} {endTime}");

            if (!DateHelper.CompareDates(date1, date2))
            {
                throw new ArgumentException("Date 1 equal or later than date 2.");
            }

            return (date1, date2);
        }

        public static int? CheckIndex(List<CodingSession> list)
        {
            if (!int.TryParse(Console.ReadLine(), out int index))
            {
                AnsiConsole.Markup("[red3_1]Invalid input.[/] Press [lime]any key[/] to continue");
                Console.ReadKey();
                return -1;
            }
            else if (index == 0) return null;

            var result = list.FindIndex(item => item.ID == index);

            if (result == -1)
            {
                AnsiConsole.Markup("[red3_1]Session not found.[/] Press [lime]any key[/] to continue");
                Console.ReadKey();
                return result;
            }

            return list[result].ID;
        }
    }
}