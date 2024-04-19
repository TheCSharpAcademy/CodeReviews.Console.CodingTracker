using CodingTracker.DAO;
using CodingTracker.Models;
using CodingTracker.Services;
using Spectre.Console;

namespace CodingTracker.Application;

public class AppSessionManager
{
    private readonly CodingSessionDao _codingSessionDAO;
    private  InputHandler _inputHandler;

    public AppSessionManager(CodingSessionDao codingSessionDAO, InputHandler inputHandler)
    {
        _codingSessionDAO = codingSessionDAO;
        _inputHandler = inputHandler;
    }

    public void Run()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Manage Coding Session Records")
                    .PageSize(10)
                    .AddChoices(Enum.GetNames(typeof(ManageSessionsMenuOptions))
                    .Select(Utilities.SplitCamelCase)));

            switch (Enum.Parse<ManageSessionsMenuOptions>(option.Replace(" ", "")))
            {
                case ManageSessionsMenuOptions.ViewAllSessions:
                    ViewSessions();
                    break;
                case ManageSessionsMenuOptions.DeleteSessionRecord:
                    DeleteSession();
                    break;
                case ManageSessionsMenuOptions.DeleteAllSessions:
                    DeleteAllSession();
                    break;
                case ManageSessionsMenuOptions.ReturnToMainMenu:
                    return;
            }
        }
    }

    private void ViewSessions()
    {
        List<CodingSessionModel> codingSessions = new List<CodingSessionModel>();

        var (periodFilter, numOfPeriods, orderByOptions) = PromptForQueryOptions(); 

        codingSessions = _codingSessionDAO.GetSessionsRecords(periodFilter, numOfPeriods, orderByOptions);

        if (codingSessions.Count == 0)
        {
            Utilities.DisplayWarningMessage("No sessions found!");
            _inputHandler.PauseForContinueInput();
            return;
        }

        Table sessionsViewTable = BuildCodingSessionsViewTable(codingSessions);

        AnsiConsole.Clear();
        AnsiConsole.Write(sessionsViewTable);
        Utilities.PrintNewLines(2);

        _inputHandler.PauseForContinueInput();
    }

    private (TimePeriod? periodFilter, int? numOfPeriods, List<(CodingSessionModel.EditableProperties, SortDirection, int)> orderByOptions) PromptForQueryOptions()
    {
        (TimePeriod? periodFilter, int? numOfPeriods) = _inputHandler.PromptForTimePeriodAndCount();
        List<(CodingSessionModel.EditableProperties, SortDirection, int)> filterAndSortOptions = _inputHandler.PromptForOrderByFilterOptions();
        
        return (periodFilter, numOfPeriods, filterAndSortOptions);
    }

    private Table BuildCodingSessionsViewTable(List<CodingSessionModel> codingSessions)
    {
        string TableTitle = "[yellow]Session Overview[/]";

        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.BorderColor(Color.Grey);
        table.Title(TableTitle);

        foreach (var property in Enum.GetValues<CodingSessionModel.SessionProperties>())
        {
            string columnName = $"[bold underline]{property}[/]";
            table.AddColumn(new TableColumn(columnName).Centered());
        }

        foreach (var session in codingSessions)
        {
            table.AddRow(
                session.Id.ToString()!,
                session.SessionDate,
                session.Duration,
                session.StartTime!,
                session.EndTime!,
                session.DateCreated,
                session.DateUpdated
            );
        }

        return table;
    }

    private void DeleteSession()
    {
        List<CodingSessionModel> sessionLogs = _codingSessionDAO.GetAllSessionRecords();
        if (!sessionLogs.Any())
        {
            Utilities.DisplayWarningMessage("No log entries available to delete.");
            _inputHandler.PauseForContinueInput();
            return;
        }

        CodingSessionModel sessionEntrySelection = _inputHandler.PromptForSessionListSelection(
            sessionLogs, "[yellow]Which log entry would you like to delete?[/]");
        
        if (AnsiConsole.Confirm($"Are you sure you want to delete this log entry (ID: {sessionEntrySelection.Id})?"))
        {
            if (_codingSessionDAO.DeleteSessionRecord(sessionEntrySelection.Id!.Value))
            {
                Utilities.DisplaySuccessMessage("Log entry successfully deleted!");
            }
            else
            {
                Utilities.DisplayWarningMessage("Failed to delete log entry. It may no longer exist or the database could be locked.");
            }
        }
        else
        {
            Utilities.DisplayCancellationMessage("]Operation cancelled.");
        }

        _inputHandler.PauseForContinueInput();
    }

    private void DeleteAllSession()
    {
        if (_codingSessionDAO.DeleteAllSessions())
        {
            Utilities.DisplaySuccessMessage("All sessions have been successfully deleted!");
            _inputHandler.PauseForContinueInput();
        }
        else
        {
            Utilities.DisplayWarningMessage("No sessions were deleted. (The table may have been empty).");
            _inputHandler.PauseForContinueInput();
        }
    }
}
