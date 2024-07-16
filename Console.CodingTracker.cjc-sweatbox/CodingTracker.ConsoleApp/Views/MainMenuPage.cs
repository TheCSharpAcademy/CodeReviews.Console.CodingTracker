using CodingTracker.ConsoleApp.Enums;
using CodingTracker.ConsoleApp.Models;
using CodingTracker.Constants;
using CodingTracker.Controllers;
using CodingTracker.Services;
using Spectre.Console;

namespace CodingTracker.ConsoleApp.Views;

/// <summary>
/// The main menu page of the application.
/// </summary>
internal class MainMenuPage : BasePage
{
    #region Constants

    private const string PageTitle = "Main Menu";

    #endregion
    #region Fields

    private readonly CodingSessionController _codingSessionController;
    private readonly CodingGoalController _codingGoalController;
    private readonly CodingGoalProgressService _codingGoalProgressService;

    #endregion
    #region Constructors

    public MainMenuPage(CodingSessionController codingSessionController, CodingGoalController codingGoalController)
    {
        _codingSessionController = codingSessionController;
        _codingGoalController = codingGoalController;
        _codingGoalProgressService = new(_codingSessionController, _codingGoalController);
    }

    #endregion
    #region Properties

    internal static IEnumerable<UserChoice> PageChoices
    {
        get
        {
            return
            [
                new(1, "Start live coding session"),
                new(2, "View coding sessions report"),
                new(3, "Filter coding sessions report"),
                new(4, "Create coding session record"),
                new(5, "Update coding session record"),
                new(6, "Delete coding session record"),
                new(7, "Set coding goal"),
                new(0, "Close application")
            ];
        }
    }

    #endregion
    #region Methods - Internal

    internal void Show()
    {
        var status = PageStatus.Opened;

        while (status != PageStatus.Closed)
        {
            AnsiConsole.Clear();

            WriteHeader(PageTitle);

            WriteCodingGoalProgress();

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<UserChoice>()
                .Title(PromptTitle)
                .AddChoices(PageChoices)
                .UseConverter(c => c.Name!)
                );

            status = PerformOption(option);
        }
    }

    #endregion
    #region Methods - Private

    private void CreateCodingSession()
    {
        // Get required data.
        var codingSession = CreateCodingSessionPage.Show();

        // If nothing is returned, user has opted to not commit.
        if (codingSession == null)
        {
            return;
        }

        // Commit to database.
        _codingSessionController.AddCodingSession(codingSession.StartTime, codingSession.EndTime);

        // Display output.
        MessagePage.Show("Create Coding Session", $"Coding session created successfully.");
    }

    private void DeleteCodingSession()
    {
        // Get required data.
        var codingSessions = _codingSessionController.GetCodingSessions();

        // Get coding session to be deleted.
        var codingSession = DeleteCodingSessionPage.Show(codingSessions);

        // If nothing is returned, user has opted to not commit.
        if (codingSession == null)
        {
            return;
        }

        // Commit to database.
        _codingSessionController.DeleteCodingSession(codingSession.Id);

        // Display output.
        MessagePage.Show("Delete Coding Session", $"Coding session deleted successfully.");
    }

    private void FilterCodingSessionsReport()
    {
        // Get raw data.
        var data = _codingSessionController.GetCodingSessions();

        // Get filter.
        var filter = ReportFilterPage.Show();

        // If nothing is returned, user has opted to not commit.
        if (filter == null)
        {
            return;
        }

        // Apply filter.
        var filteredData = filter.Apply(data);

        // Configure table data.
        string tableTitle = $"Coding Session Report {(filter.StartDate.HasValue && filter.EndDate.HasValue ? $"for range: {filter.StartDate:yyyy-MM-dd} - {filter.EndDate:yyyy-MM-dd}" : "")}";
        var table = new Table
        {
            Title = new TableTitle(tableTitle)
        };
        table.AddColumn("Start");
        table.AddColumn("End");
        table.AddColumn("Duration");

        foreach (var x in filteredData)
        {
            table.AddRow(x.StartDateTimeString, x.EndDateTimeString, x.DurationString);
        }

        table.Caption = new TableTitle(filteredData.Any()
            ? $"Total: {filteredData.Sum(x => x.Duration):F2}{Environment.NewLine}Average: {filteredData.Average(x => x.Duration):F2}"
            : "No coding sessions found.");

        // Fill up window.
        table.Expand();

        // Display report.
        MessagePage.Show("Coding Session Report", table);
    }

    private void LiveCodingSession()
    {
        // Get required data.
        var codingSession = LiveCodingSessionPage.Show();

        // Commit to database.
        _codingSessionController.AddCodingSession(codingSession.StartTime, codingSession.EndTime);

        // Display output.
        MessagePage.Show("Live Coding Session", $"Coding session created successfully.");
    }

    private PageStatus PerformOption(UserChoice option)
    {
        switch (option.Id)
        {
            case 0:

                // Close application.
                return PageStatus.Closed;

            case 1:

                // Start live coding session.
                LiveCodingSession();
                break;

            case 2:
                // View coding sessions report.
                ViewCodingSessionsReport();
                break;

            case 3:

                // Filter coding sessions report.
                FilterCodingSessionsReport();
                break;

            case 4:

                // Create coding session record.
                CreateCodingSession();
                break;

            case 5:

                // Update coding session record.
                UpdateCodingSession();
                break;

            case 6:

                // Delete coding session record.
                DeleteCodingSession();
                break;

            case 7:

                // Set coding goal.
                SetCodingGoal();
                break;

            default:

                // Do nothing, but remain on this page.
                break;
        }

        return PageStatus.Opened;
    }

    private void SetCodingGoal()
    {
        // Get coding goal.
        var codingGoal = SetCodingGoalPage.Show();

        // If nothing is returned, user has opted to not commit.
        if (codingGoal == null)
        {
            return;
        }

        // Commit to database.
        _codingGoalController.SetCodingGoal(codingGoal.WeeklyDurationInHours);

        // Display output.
        MessagePage.Show("Set Coding Goal", $"Coding goal set successfully.");
    }

    private void UpdateCodingSession()
    {
        // Get required data.
        var codingSessions = _codingSessionController.GetCodingSessions();

        // Get updated coding session.
        var codingSession = UpdateCodingSessionPage.Show(codingSessions);

        // If nothing is returned, user has opted to not commit.
        if (codingSession == null)
        {
            return;
        }

        // Commit to database.
        _codingSessionController.SetCodingSession(codingSession.Id, codingSession.StartTime, codingSession.EndTime);

        // Display output.
        MessagePage.Show("Update Coding Session", $"Coding session updated successfully.");
    }

    private void ViewCodingSessionsReport()
    {
        // Get raw data.
        var data = _codingSessionController.GetCodingSessions();

        // Configure table data.
        var table = new Table
        {
            Title = new TableTitle("Coding Session Report")
        };
        table.AddColumn("ID");
        table.AddColumn("Start");
        table.AddColumn("End");
        table.AddColumn("Duration");

        foreach (var x in data)
        {
            table.AddRow(x.Id.ToString(), x.StartTime.ToString(StringFormat.DateTime), x.EndTime.ToString(StringFormat.DateTime), x.Duration.ToString("F2"));
        }

        table.Caption = new TableTitle(data.Count > 0
            ? $"Total: {data.Sum(x => x.Duration):F2}{Environment.NewLine}Average: {data.Average(x => x.Duration):F2}"
            : "No coding sessions found.");

        // Fill up window.
        table.Expand();

        // Display report.
        MessagePage.Show("Coding Session Report", table);
    }

    private void WriteCodingGoalProgress()
    {
        var progress = _codingGoalProgressService.GetCodingGoalProgress();
        AnsiConsole.WriteLine($"Welcome, {progress}");
        AnsiConsole.WriteLine();
    }

    #endregion
}
