using CodingTracker.DAO;
using CodingTracker.Models;
using CodingTracker.Services;
using Spectre.Console;

namespace CodingTracker.Application;

public class AppNewLogManager
{
    private readonly CodingSessionDAO _codingSessionDAO;
    private InputHandler _inputHandler;

    public AppNewLogManager(CodingSessionDAO codingSessionDAO, InputHandler inputHandler)
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
                    .Title("Add New Session Log Records")
                    .PageSize(10)
                    .AddChoices(Enum.GetNames(typeof(LogManualSessionMenuOptions))
                    .Select(Utilities.SplitCamelCase)));

            switch (Enum.Parse<LogManualSessionMenuOptions>(option.Replace(" ", "")))
            {
                case LogManualSessionMenuOptions.LogManualExternalSession:
                    LogSessionByStartEndTimes();
                    break;
                case LogManualSessionMenuOptions.ReturnToMainMenu:
                    return;
            }
        }
    }

    private void LogSessionByStartEndTimes()
    {
        AnsiConsole.Clear();

        DateTime startTime = _inputHandler.PromptForDate($"Enter the Start Time for the log entry {ConfigSettings.DateFormatLong}:", DatePrompt.Long);
        DateTime endTime;

        do
        {
            endTime = _inputHandler.PromptForDate($"Enter the End Time for the session {ConfigSettings.DateFormatLong}:", DatePrompt.Long);
            if (endTime <= startTime)
            {
                Utilities.DisplayWarningMessage("End time must be after start time. Please enter a valid end time.");
            }
        } while (endTime <= startTime);

        // Create new coding session object via constructor (duration automatically calculated)
        CodingSessionModel newSession = new CodingSessionModel(startTime, endTime);

        bool success = _codingSessionDAO.InsertSessionAndUpdateGoals(newSession);
        if (success)
        {
            string successMessage = $"[green]Session successfully logged![/]";
            Utilities.DisplaySuccessMessage(successMessage);
        }
        else
        {
            Utilities.DisplayWarningMessage("Failed to log the session. Please try again or check the system logs.");
        }

        _inputHandler.PauseForContinueInput();
    }
}
