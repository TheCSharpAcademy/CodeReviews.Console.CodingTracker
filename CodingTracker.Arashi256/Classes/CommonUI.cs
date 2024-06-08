using CodingTracker.Arashi256.Models;
using Spectre.Console;

namespace CodingTracker.Arashi256.Classes
{
    internal class CommonUI
    {
        public static void Pause()
        {
            AnsiConsole.Markup("[cornflowerblue]Press any key to continue...[/]");
            Console.ReadKey(true);
        }

        public static DateTime? GetUserDateTimeDialog()
        {
            string format = "dd-MM-yy HH:mm:ss";
            DateTime? dateTime = null;
            while (!dateTime.HasValue)
            {
                AnsiConsole.MarkupLine($"[steelblue1_1]Note: Enter 'Q'/'q' to abort and return to main menu[/]");
                var userInput = AnsiConsole.Prompt(new TextPrompt<string>($"Enter a date/time in the format '{format}':").PromptStyle("white"));
                if (userInput.ToLower() == "q")
                {
                    return null;
                }
                else
                {
                    if (DateTime.TryParseExact(userInput, format, null, System.Globalization.DateTimeStyles.None, out DateTime result))
                    {
                        dateTime = result;
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Invalid date/time format. Please enter the date/time in the specified format.[/]");
                    }
                }
            }
            return dateTime;
        }

        public static int GetHoursDialog()
        {
            int hours = 0;
            hours = GetPositiveWholeNumber("How many hours coding is your goal?");
            return hours;
        }

        public static int GetPositiveWholeNumber(string prompt)
        {
            int result;
            bool isValid;
            do
            {
                string input = AnsiConsole.Ask<string>(prompt);
                isValid = int.TryParse(input, out result) && result > 0;
                if (!isValid)
                {
                    AnsiConsole.MarkupLine("[red]Please enter a positive whole number.[/]");
                }
            } while (!isValid);
            return result;
        }

        public static int MenuOption(string question, int min, int max)
        {
            int selectedValue = 0;
            var userInput = AnsiConsole.Ask<int>(question);
            selectedValue = userInput;
            if (selectedValue < min || selectedValue > max)
            {
                AnsiConsole.MarkupLine("[red]Invalid input. Please enter a value within the specified range.[/]");
            }
            return selectedValue;
        }

        public static Utility.SortOrder GetSortOrderDialog()
        {
            AnsiConsole.MarkupLine("[white]Ascending (ASC): Orders data from earliest to latest.[/]");
            AnsiConsole.MarkupLine("[white]Descending (DESC): Orders data from latest to earliest.[/]");
            var choice = AnsiConsole.Prompt(
                            new SelectionPrompt<Utility.SortOrder>()
                                .Title("Please choose the sorting order:")
                                .AddChoices(Utility.SortOrder.ASC, Utility.SortOrder.DESC));
            AnsiConsole.MarkupLine($"You selected: [lime]{choice}[/]");
            return choice;
        }

        public static void DisplayCodingSessions(List<CodingSession> codingSessions)
        {
            Table tblSessionList = new Table();
            tblSessionList.AddColumn(new TableColumn("[white]ID[/]").LeftAligned());
            tblSessionList.AddColumn(new TableColumn("[white]Start Date/Time[/]").LeftAligned());
            tblSessionList.AddColumn(new TableColumn("[white]End Date/Time[/]").RightAligned());
            tblSessionList.AddColumn(new TableColumn("[white]Duration[/]").RightAligned());
            if (codingSessions.Count > 0)
            {
                for (int i = 0; i < codingSessions.Count; i++)
                {
                    tblSessionList.AddRow($"[white]{i + 1}[/]", $"[white]{codingSessions[i].StartDateTime:dd-MM-yy HH:mm:ss}[/]", $"[white]{codingSessions[i].EndDateTime:dd-MM-yy HH:mm:ss}[/]", $"[white]{codingSessions[i].Duration:hh:mm:ss}[/]");
                }
                tblSessionList.AddRow("", "", "", "");
                tblSessionList.AddRow($"", "", "[skyblue2]Total Time[/]", $"[skyblue1]{Utility.SumCodingSessions(codingSessions.ToArray())}[/]");
                tblSessionList.AddRow($"", "", "[skyblue2]Average Time[/]", $"[skyblue1]{Utility.AverageCodingSessions(codingSessions.ToArray())}[/]");
            }
            else
            {
                tblSessionList.AddRow("[red]No sessions found[/]");
            }
            tblSessionList.Alignment(Justify.Left);
            AnsiConsole.Write(tblSessionList);
        }

        public static DateTime[]? GetUserCodingSessionDatesDialog()
        {
            DateTime? startTime, endTime;
            DateTime[] result = new DateTime[2];
            bool hasError = false;
            do
            {
                startTime = CommonUI.GetUserDateTimeDialog();
                if (startTime != null)
                {
                    AnsiConsole.MarkupLine($"[white]You entered[/] [green]{startTime:dd-MM-yy HH:mm:ss}[/] [white]as the start time.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[yellow]Operation cancelled.[/]");
                    return null;
                }
                endTime = CommonUI.GetUserDateTimeDialog();
                if (endTime != null)
                {
                    AnsiConsole.MarkupLine($"[white]You entered[/] [green]{endTime:dd-MM-yy HH:mm:ss}[/] [white]as the end time.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[yellow]Operation cancelled.[/]");
                    return null;
                }
                if (startTime > endTime)
                {
                    hasError = true;
                    AnsiConsole.MarkupLine($"[red]'{startTime}' cannot be after '{endTime}' Please try again[/].");
                }
                else
                    hasError = false;
            } while (hasError);
            result[0] = (DateTime)startTime;
            result[1] = (DateTime)endTime;
            return result;
        }
    }
}
