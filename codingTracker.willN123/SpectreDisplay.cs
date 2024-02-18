using Spectre.Console;

namespace CodingTracker
{
    internal class SpectreDisplay
    {
        readonly Rule line = new Rule().RuleStyle("aqua");

        void Header()
        {
            Console.Clear();

            AnsiConsole.Write(line);
            AnsiConsole.Write(new FigletText("Coding Tracker")
                .Color(Color.Aqua)
                .Centered());
            AnsiConsole.Write(line);
        }

        public string MainMenuSelection()
        {
            Header();

            var menuSelection = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Select a menu option with the [aqua]arrow keys[/], then press [aqua]Enter[/].")
                .AddChoices(["View", "Add", "Update", "Delete", "Exit"])   );
                //.HighlightStyle(Color.Aqua));

            return menuSelection;
        }

        public void DisplayTable(List<CodingSession> sessions)
        {
            var table = new Table()
                .AddColumn(new TableColumn(header: "Id #").Centered())
                .AddColumn(new TableColumn(header: "Date").Centered())
                .AddColumn(new TableColumn(header: "Start").Centered())
                .AddColumn(new TableColumn(header: "End").Centered())
                .AddColumn(new TableColumn(header: "Total").Centered());

            foreach (var session in sessions)
            {
                table.AddRow(session.id.ToString(), session.date, session.startTime, session.endTime, session.totalTime);
            }

            AnsiConsole.Write(table);
        }

        public void WaitForEnter()
        {
            AnsiConsole.Prompt(new TextPrompt<string>
                ("Press [aqua]Enter[/] to return to menu.")
                .AllowEmpty());
        }

        public string DatePrompt()
        {
            var dateInput = AnsiConsole.Ask<string>
                ("Enter [aqua]date[/]: [aqua](format: dd-mm-yy)[/]");

            return dateInput;
        }

        public string StartTimePrompt()
        {
            var startInput = AnsiConsole.Ask <string>
                ("Enter [aqua]start[/] time: [aqua](24hr time in format: hhmm)[/]");

            return startInput;
        }

        public string EndTimePrompt()
        {
            var endInput = AnsiConsole.Ask<string>
                ("Enter [aqua]end[/] time: [aqua](24hr time in format: hhmm)[/]");
            
            return endInput;
        }

        public void ShowRecordInput(string date, string start, string end, string total)
        {
            AnsiConsole.Write(new Table()
                .AddColumn(new TableColumn(header: "Date"))
                .AddColumn(new TableColumn(header: "Start"))
                .AddColumn(new TableColumn(header: "End"))
                .AddColumn(new TableColumn(header: "Total"))
                .AddRow(date, start, end, total));
        }

        public bool ConfirmEntry()
        {
            if ( AnsiConsole.Prompt(new ConfirmationPrompt
                    ("Continue [aqua]y[/] or Discard [aqua]n[/]?")
                    .ChoicesStyle("aqua")
                    .HideDefaultValue())
               )
            {
                return true;
            }

            return false;
        }

        public string IdPrompt()
        {
            var idInput = AnsiConsole.Ask<string>("Enter [aqua]Id#[/]:");

            return idInput;
        }

        public void InvalidEntry()
        {
            AnsiConsole.Markup("[aqua]Invalid[/] entry. Try again.\n");
        }
    }
}
