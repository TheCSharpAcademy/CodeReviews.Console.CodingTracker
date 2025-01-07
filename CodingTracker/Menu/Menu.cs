using Spectre.Console;

internal class Menu
{
    internal static Dictionary<string, Action> menuActions = new()
    {
        { "Start coding session", SessionStopwatch.Start },
        { "Show all coding sessions", RecordRead.ShowAllRecords },
        { "Create new record", RecordCreate.Create },
        { "Delete record", RecordDelete.Delete },
        { "Update record", RecordUpdate.Update },
        { "Create quick report", QuickReport.ShowMenu },
        { "Create a report for period of time", RecordReport.ShowReportForPeriodOfTime },
        { "[yellow]Make random records[/]", MockSessionRecords.CreateRandomRecords },
        { "[red]Delete all records[/]", RecordDelete.DeleteAllRecords },
        { "[cyan]Set new goal[/]", Goals.SetNewGoal },
        { "Exit", () =>
            {
                Console.Clear();
                AnsiConsole.MarkupLine("[yellow]Goodbye![/]");
                Environment.Exit(0);
            }
        }
    };

    internal static void ShowMainMenu()
    {
        while (true)
        {
            Goals.ShowProgress();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Choose an action: ")
                .PageSize(10)
                .AddChoices(menuActions.Keys));
            menuActions[choice]();
        }
    }
}
