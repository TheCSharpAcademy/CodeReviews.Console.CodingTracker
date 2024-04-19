using CodingTracker.DAO;
using CodingTracker.Services;
using Spectre.Console;

namespace CodingTracker.Application;

public class AppReportManager
{
    private readonly CodingSessionDao _codingSessionDAO;
    private InputHandler _inputHandler;

    public AppReportManager(CodingSessionDao codingSessionDAO, InputHandler inputHandler)
    {
        _codingSessionDAO = codingSessionDAO;
        _inputHandler = inputHandler;
    }

    public void Run()
    {
        AnsiConsole.Clear();
        GenerateReport();
        _inputHandler.PauseForContinueInput();
    }

    private void GenerateReport()
    {
        var period = AnsiConsole.Prompt(
            new SelectionPrompt<TimePeriod>()
                .Title("Choose the period for the report:")
                .AddChoices(TimePeriod.Days, TimePeriod.Weeks, TimePeriod.Years));

        int numberOfPeriods = _inputHandler.PromptForPositiveInteger("Enter number of periods to include: ");

        DisplayReport(period, numberOfPeriods);
    }

    private void DisplayReport(TimePeriod period, int numberOfPeriods)
    {
        var data = _codingSessionDAO.GetSessionStatistics(period, numberOfPeriods);
        AnsiConsole.Clear();
        AnsiConsole.Markup($"[bold]Report for last {numberOfPeriods} {period}[/]\n");
        Utilities.PrintNewLines(1);
        AnsiConsole.Markup($"Total Sessions: [bold]{data.TotalSessions}[/]\n");
        AnsiConsole.Markup($"Total Duration: [bold]{data.TotalSessions * data.AverageDuration} hours[/]\n");
        AnsiConsole.Markup($"Average Duration: [bold]{data.AverageDuration} hours[/]\n");
        Utilities.PrintNewLines(2);
    }
}
