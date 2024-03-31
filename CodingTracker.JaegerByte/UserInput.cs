using Spectre.Console;
namespace CodingTracker.JaegerByte
{
    internal class UserInput
    {
        public MenuOption GetOption()
        {
            SelectionPrompt<MenuOption> selectionPrompt = new SelectionPrompt<MenuOption>();
            selectionPrompt.Title = "Select option";
            foreach (MenuOption option in Enum.GetValues(typeof(MenuOption)))
            {
                selectionPrompt.AddChoice(option);
            }
            return AnsiConsole.Prompt(selectionPrompt);
        }
        public string GetDateInput(string prompt)
        {
            return AnsiConsole.Ask<string>(prompt);
        }
        public string GetIndexInput(string prompt)
        {
            List<CodingSession> sessions = Program.databaseService.GetCodingSessions();
            AnsiConsole.Write(GetAllSessionsGrid(sessions));
            return AnsiConsole.Ask<string>(prompt);
        }
        public Grid GetAllSessionsGrid(List<CodingSession> sessionList)
        {
            Grid grid = new Grid();

            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();

            grid.AddRow(new Text[]
            {
                new Text("ID", new Style(Color.Blue)).LeftJustified(),
                new Text("Start Time", new Style(Color.Green)).Centered(),
                new Text("End Time", new Style(Color.Red)).Centered(),
                new Text("Duration", new Style(Color.Yellow)).LeftJustified(),
            });
            foreach (CodingSession item in sessionList)
            {
                grid.AddRow(new Text[]
                {
                    new Text(item.ID.ToString()),
                    new Text(item.StartTime.ToString("g")),
                    new Text(item.EndTime.ToString("g")),
                    new Text(item.Duration.ToString("hh\\:mm")),
                });
            }
            return grid;
        }
    }
}
