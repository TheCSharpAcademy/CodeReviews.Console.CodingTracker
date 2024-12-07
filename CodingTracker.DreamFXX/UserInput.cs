using Spectre.Console;
using System.Globalization;

namespace CodingTracker.DreamFXX
{
    internal class UserInput
    {
        Validation _dateTimeValidation = new();
        private string? Date { get; set; } = string.Empty;
        private string? StartTime { get; set; } = string.Empty;
        private string? EndTime { get; set; } = string.Empty;

        public string GetDate()
        {
            this.Date = AnsiConsole.Ask<string>(
                "[yellow]Please, enter the date of your session. Specify date in this exact format![/]\n-[green](dd-MM-yy)[/] -> ");
            while (!_dateTimeValidation.IsValidDate(this.Date))
            {
                AnsiConsole.MarkupLine("Invalid date! Expected format (mm-dd-yy)");
                this.Date = Console.ReadLine();
            }
            return this.Date;
        }

        public string GetStartTime()
        {
            this.StartTime = AnsiConsole.Ask<string>
                ("[yellow]Enter the time your session started. Specify time in this format![/]\n-[green](hh:mm)[/] -> ");

            while (!_dateTimeValidation.IsValidStartTime(this.StartTime))
            {
                AnsiConsole.MarkupLine("[red]Invalid time! Time must be in this format - (hh:mm):[/] ");
                this.StartTime = Console.ReadLine();
            }

            return this.StartTime;
        }

        public string GetEndTime()
        {
            this.EndTime = AnsiConsole.Ask<string>
                ("[yellow]Enter the time your session ended. Specify time in this format![/]\n-[green](hh:mm)[/] -> ");
            while (!_dateTimeValidation.IsValidStartTime(this.EndTime))
            {
                AnsiConsole.MarkupLine("[red]Invalid time! Time must be in this format - (hh:mm):[/] ");
                this.EndTime = Console.ReadLine();
            }

            return this.EndTime;
        }

        public string GetDuration()
        {
            DateTime parsedStartTime = DateTime.ParseExact(StartTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
            DateTime parsedEndTime = DateTime.ParseExact(EndTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);

            TimeSpan duration = parsedEndTime.Subtract(parsedStartTime);
            if (duration < TimeSpan.Zero)
            {
                duration += TimeSpan.FromDays(1);
            }
            return duration.ToString();
        }
    }
}
