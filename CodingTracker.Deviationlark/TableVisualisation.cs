using Spectre.Console;

namespace CodingTracker
{
    internal class TableVisualisation
    {
        internal void ShowTable(List<CodingSession> tableData)
        {

            foreach (var element in tableData)
            {
                string date = element.Date;
                if (DateTime.TryParseExact(date, "dd-MM-yy", null, System.Globalization.DateTimeStyles.None, out _)) continue;
                string substring1 = date.Substring(0, 2);
                string substring2 = date.Substring(2, 2);
                string substring3 = date.Substring(4, 2);

                date = substring1 + '-' + substring2 + '-' + substring3;
                string[] splitDate = date.Split('-');
                Array.Reverse(splitDate);

                element.Date = string.Join('-', splitDate);
            }
            Console.WriteLine("\n\n");
            var table = new Table();

            table.Title("[green]Coding[/]");

            table.AddColumn("[blue]Id[/]");
            table.AddColumn("[blue]Date[/]");
            table.AddColumn("[blue]Duration[/]");

            foreach (var session in tableData)
            {
                table.AddRow($"[tan]{session.Id}[/]", $"[tan]{session.Date}[/]", $"[tan]{session.Duration}[/]");
            }

            AnsiConsole.Write(table);
        }

        internal void FilteredTable(List<CodingSession> tableData)
        {
            foreach (var element in tableData)
            {
                string date = element.Date;
                if (DateTime.TryParseExact(date, "dd-MM-yy", null, System.Globalization.DateTimeStyles.None, out _)) continue;
                string substring1 = date.Substring(0, 2);
                string substring2 = date.Substring(2, 2);
                string substring3 = date.Substring(4, 2);

                date = substring1 + '-' + substring2 + '-' + substring3;
                string[] splitDate = date.Split('-');
                Array.Reverse(splitDate);

                element.Date = string.Join('-', splitDate);
            }
            Console.WriteLine("\n\n");
            var table = new Table();

            table.Title("[green]Coding[/]");

            table.AddColumn("[blue]Id[/]");
            table.AddColumn("[blue]Date[/]");
            table.AddColumn("[blue]Duration[/]");

            foreach (var session in tableData)
            {
                table.AddRow($"[tan]{session.Id}[/]", $"[tan]{session.Date}[/]", $"[tan]{session.Duration}[/]");
            }

            AnsiConsole.Write(table);
        }

        internal void ShowGoalsTable(List<Goals> tableData)
        {
            Console.WriteLine("\n\n");
            var table = new Table();

            table.Title("[green]Coding Goals[/]");

            table.AddColumn("[blue]Id[/]");
            table.AddColumn("[blue]Hours[/]");
            table.AddColumn("[blue]Due Date[/]");
            table.AddColumn("[blue]Remaining Days[/]");
            table.AddColumn("[blue]Remaining Hours[/]");
            table.AddColumn("[blue]Hours/Minutes Per Day[/]");

            foreach (var goal in tableData)
            {
                if (goal.RemainingDays != "Completed")
                {
                    if (Convert.ToDouble(goal.HoursPerDay) < 1 && Convert.ToDouble(goal.HoursPerDay) > 0)
                    {
                        goal.HoursPerDay = Convert.ToInt32(Convert.ToDouble(goal.HoursPerDay) * 60.0).ToString();
                        table.AddRow($"[tan]{goal.Id}[/]", $"[tan]{goal.Hours}[/]", $"[tan]{goal.Date}[/]",
                        $"[tan]{goal.RemainingDays}[/]", $"[tan]{goal.RemainingHours}[/]", $"[tan]{goal.HoursPerDay} minutes[/]");
                    }
                }

                else
                {
                    if (goal.RemainingDays != "Completed") goal.HoursPerDay = Convert.ToInt32(Convert.ToDouble(goal.HoursPerDay)).ToString();
                    table.AddRow($"[tan]{goal.Id}[/]", $"[tan]{goal.Hours}[/]", $"[tan]{goal.Date}[/]",
                    $"[tan]{goal.RemainingDays}[/]", $"[tan]{goal.RemainingHours}[/]", $"[tan]{goal.HoursPerDay}[/]");
                }
            }

            AnsiConsole.Write(table);
        }
    }
}