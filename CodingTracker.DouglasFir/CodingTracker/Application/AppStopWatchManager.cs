using CodingTracker.DAO;
using CodingTracker.Models;
using CodingTracker.Services;
using Spectre.Console;

namespace CodingTracker.Application;

public class AppStopwatchManager
{
    private readonly CodingSessionDAO _codingSessionDAO;
    private InputHandler _inputHandler;
    private CodingSessionModel? _sessionModel;
    private bool _returnToMain;
    private bool _stopwatchRunning;
    private DateTime _startTime;
    private Panel? _sessionPanel;

    public AppStopwatchManager(CodingSessionDAO codingSessionDAO, InputHandler inputHandler)
    {
        _codingSessionDAO = codingSessionDAO;
        _inputHandler = inputHandler;
        _stopwatchRunning = false;
        _returnToMain = false;
    }

    public void Run()
    {
        while (!_returnToMain)
        {
            AnsiConsole.Clear();
            if (_stopwatchRunning)
            {
                DisplaySessionPanel();
                HandleActiveSessionOptions();
            }
            else
            {
                HandleInactiveSessionOptions();
            }
        }
    }

    private void DisplaySessionPanel()
    {
        AnsiConsole.Write(_sessionPanel!);
        Utilities.PrintNewLines(3);
    }

    private void HandleActiveSessionOptions()
    {
        var options = new List<StartSessionMenuOptions>
        {
            StartSessionMenuOptions.RefreshElapsedTime,
            StartSessionMenuOptions.EndCurrentSession
        };

        PromptForSessionAction(options);
    }

    private void HandleInactiveSessionOptions()
    {
        var options = new List<StartSessionMenuOptions>
        {
            StartSessionMenuOptions.StartSession,
            StartSessionMenuOptions.ReturnToMainMenu
        };

        PromptForSessionAction(options);
    }

    private void PromptForSessionAction(List<StartSessionMenuOptions> options)
    {
        StartSessionMenuOptions selectedOption = AnsiConsole.Prompt(
            new SelectionPrompt<StartSessionMenuOptions>()
                .Title("Start and Stop New Coding Session")
                .PageSize(10)
                .AddChoices(options)
                .UseConverter(selectedOption => Utilities.SplitCamelCase(selectedOption.ToString())));

        ExecuteSelectedOption(selectedOption);
    }

    private void ExecuteSelectedOption(StartSessionMenuOptions option)
    {
        switch (option)
        {
            case StartSessionMenuOptions.StartSession:
                HandleNewSessionAction();
                break;
            case StartSessionMenuOptions.RefreshElapsedTime:
                UpdateSessionPanel();
                break;
            case StartSessionMenuOptions.EndCurrentSession:
                HandleEndSessionAction();
                break;
            case StartSessionMenuOptions.ReturnToMainMenu:
                _returnToMain = true;
                break;
        }
    }

    private void HandleNewSessionAction()
    {
        InitiateSessionStart();
        UpdateSessionPanel();
    }

    private void InitiateSessionStart()
    {
        _startTime = DateTime.Now;
        _sessionModel = new CodingSessionModel(_startTime, _startTime);
        _stopwatchRunning = true;
    }

    private void UpdateSessionPanel()
    {
        string sessionPanelInformation = BuildNewPanelInformation();
        BuildNewPanel(sessionPanelInformation);
    }

    private string BuildNewPanelInformation()
    {
        DateTime currentTime = DateTime.Now;
        TimeSpan elapsedTime = currentTime - _startTime;

        string panelSessionTitle = $"\n[bold]Current Session:[/] \n\n";
        string panelStartTime = $"[underline]Start Time[/]:       [royalblue1]{_startTime.ToString(ConfigSettings.DateFormatLong)}[/]\n";
        string panelElapsedTime = $"[underline]Elapsed Time[/]:     [steelblue1]{elapsedTime.ToString(@"hh\:mm\:ss")}[/]\n\n";
        string panelLastUpdated = $"Last Updated:     [darkgoldenrod]{currentTime.ToString(ConfigSettings.DateFormatLong)}[/]";

        string panelInformation = panelSessionTitle + panelStartTime + panelElapsedTime + panelLastUpdated;

        return panelInformation;
    }

    private void BuildNewPanel(string panelInformation)
    {
        _sessionPanel = new Panel(new Markup(panelInformation));
        _sessionPanel.Header = new PanelHeader("[mediumspringgreen]  Session Running  [/]", Justify.Center);
        _sessionPanel.Padding = new Padding(2, 2, 2, 2);
        _sessionPanel.Border = BoxBorder.Rounded;
        _sessionPanel.Expand = true;
    }

    private void HandleEndSessionAction()
    {
        AnsiConsole.Clear();

        if (!_inputHandler.ConfirmAction("Are you sure you want to end the current session?")) 
            return;

        ProcessEndSessionActions();
        _inputHandler.PauseForContinueInput();
    }

    private void ProcessEndSessionActions()
    {
        try
        {
            UpdateSessionEndValueAttributes();

            _codingSessionDAO.InsertSessionAndUpdateGoals(_sessionModel!);
        }
        catch (Exception ex)
        {
            string errorMessage = "Error ending session";
            Utilities.DisplayExceptionErrorMessage(errorMessage, ex.Message);
            _inputHandler.PauseForContinueInput();
            return;
        }

        DisplaySuccessfulEndSessionView();
    }

    private void UpdateSessionEndValueAttributes()
    {
        DateTime endTime = DateTime.Now;
        _sessionModel!.SetEndTime(endTime);
        _sessionModel.SetDuration(_startTime, endTime);
        _stopwatchRunning = false;
    }

    private void DisplaySuccessfulEndSessionView()
    {
        string successMessage = "Session ended and logged successfully";
        Utilities.DisplaySuccessMessage(successMessage);
    }
}
