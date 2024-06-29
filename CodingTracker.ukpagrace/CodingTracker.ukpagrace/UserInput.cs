using Spectre.Console;
using System.Globalization;

namespace CodingTracker.ukpagrace
{
    internal class UserInput
    {
        public DateTime GetStartDate()
        {
            var input = AnsiConsole.Ask<string>("[blue]Input your start time in the format of YYYY-MM-DD HH:MM[/]");

            DateTime startDate;
            while (!DateTime.TryParseExact(input, "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                input = AnsiConsole.Ask<string>("[blue]Start Date should be in the format of YYYY-MM-DD HH:MM[/]");
            }
            return startDate;
        }

        public DateTime GetEndDate()
        {
            var input = AnsiConsole.Ask<string>("[blue]Input your end date in the format of YYYY-MM-DD HH:MM[/]");

            DateTime endDate;
            while (!DateTime.TryParseExact(input, "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                input = AnsiConsole.Ask<string>("[blue]End Date should be in the format of YYYY-MM-DD HH:MM[/]");
            }
            return endDate;
        }

        public string DateInput(string filterFormat)
        {
            var input = AnsiConsole.Ask<string>($"[blue]Input your date in the format of {filterFormat}[/]");

            DateTime date;
            while (!DateTime.TryParseExact(input, filterFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                input = AnsiConsole.Ask<string>($"[blue]Date should be in the format of {filterFormat}[/]");
            }
            return date.ToString(filterFormat);
        }
        public bool ConfirmAction(string message)
        {
            if (!AnsiConsole.Confirm($"[blue]{message}[/]"))
            {
                AnsiConsole.MarkupLine("Ok... :(");
                return false;
            }

            return true;
        }

        public int GetNumberInput(string message)
        {

            var input = AnsiConsole.Ask<string>($"[blue]{message}[/]");

            while (!int.TryParse(input, out _) && Convert.ToInt32(input) < 0)
            {
                input = AnsiConsole.Ask<string>("[blue]Enter a valid input[/]");
            }
            return Convert.ToInt32(input);
        }

        public string GetOrderInput()
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Would you like to order in ascending or descending order")
                    .PageSize(3)
                    .AddChoices("Ascending", "Descending")
            );
            if (option == null) option = "ascending";
            string order = (option == "ascending") ? "ASC" : "DESC";
            return order;
        }

        public (string, string) GetRange(string filterFormat)
        {
            var firstRange = AnsiConsole.Ask<string>($"Enter the first range value in the format of [yellow]{filterFormat}[/]");

            while (!DateTime.TryParseExact(firstRange, filterFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                firstRange = AnsiConsole.Ask<string>($"[blue]Range should be in the format of {filterFormat}[/]");
            }
            var secondRange = AnsiConsole.Ask<string>($"Enter the second range value in the format of [yellow]{filterFormat}[/]");

            while (!DateTime.TryParseExact(secondRange, filterFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                secondRange = AnsiConsole.Ask<string>($"[blue]Rangeshould be in the format of f{filterFormat}[/]");
            }

            return (firstRange, secondRange);
        }
    }
}
