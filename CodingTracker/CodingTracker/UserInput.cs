using Spectre.Console;
using System.Globalization;
namespace CodingTracker
{
    public class UserInput
    {
        string helper1;
        string helper2;
        Validation validation;
        CodingSession session;
        DbController controller;
        public UserInput(CodingSession _session, DbController _controller)
        {
            validation = new Validation(string.Empty);
            this.session = _session;
            this.controller = _controller;
        }

        public void DateOfStart()
        {
            AnsiConsole.MarkupLine("[green]Please type in the date of the start below in yyyy.mm.dd. format (like this: 2024.06.29. separated by dots):[/]");
            helper1 = Console.ReadLine();
            if (validation.ValidString(helper1))
            {
                session.StartDate = helper1;
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]Please try again![/]");
                Thread.Sleep(1000);
                controller.InsertRecords();
            }
        }
        public void DateOfEnd()
        {
            AnsiConsole.MarkupLine("[green]Please type in the date of the end below in yyyy.mm.dd. format (like this: 2024.06.29. separated by dots):[/]");
            helper2 = Console.ReadLine();
            if (validation.ValidString(helper2))
            {
                session.EndDate = helper2;
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]Please try again![/]");
                Thread.Sleep(1000);
                controller.InsertRecords();
            }
        }
        public string Duration(string startDate, string endDate)
        {
            if (DateTime.TryParseExact(startDate, "yyyy.MM.dd.", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime start) &&
                DateTime.TryParseExact(endDate, "yyyy.MM.dd.", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime end))
            {
                if (end < start)
                {
                    return "";
                }

                TimeSpan duration = end - start;

                return $"{duration.Days}";
            }
            else
            {
                return "";
            }
        }
    }
}
