using CodingTracker.ConsoleApp.Models;
using CodingTracker.ConsoleApp.Services;
using CodingTracker.Constants;
using CodingTracker.Models;
using CodingTracker.Services;
using Spectre.Console;

namespace CodingTracker.ConsoleApp.Views;

/// <summary>
/// Page which allows users to update a CodingSession.
/// </summary>
internal class UpdateCodingSessionPage : BasePage
{
    #region Constants

    private const string PageTitle = "Update Coding Session";

    private const string PromptTitle = "Select an option...";

    #endregion
    #region Properties

    internal static IEnumerable<UserChoice> PageChoices
    {
        get
        {
            return
            [
                new(0, "Close page"),                
            ];
        }
    }

    #endregion
    #region Methods - Internal

    internal static CodingSession? Show(List<CodingSession> codingSessions)
    {
        AnsiConsole.Clear();

        WriteHeader(PageTitle);

        var option = GetOption(codingSessions);
        
        // Close page.
        if (option.Id == 0)
        {
            return null;
        }

        // Get the updated CodingSession from user input.
        var codingSession = codingSessions.First(x => x.Id == option.Id);
        codingSession = GetUpdatedCodingSession(codingSession);
        
        return codingSession;
    }

    private static CodingSession? GetUpdatedCodingSession(CodingSession codingSession)
    {
        // Configure table data.
        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Start Time");
        table.AddColumn("End Time");
        table.AddColumn("Duration");
        table.AddRow(codingSession.Id.ToString(), codingSession.StartTime.ToString(StringFormat.DateTime), codingSession.EndTime.ToString(StringFormat.DateTime), codingSession.Duration.ToString("F2"));
        
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();

        string dateTimeFormat = StringFormat.DateTime;

        // Get the updated start date time of the CodingSession.
        DateTime? start = UserInputService.GetDateTime(
            $"Enter the start date and time, format [blue]{dateTimeFormat}[/], or [blue]0[/] to return to main menu: ",
            dateTimeFormat,
            input => UserInputValidationService.IsValidCodingSessionStartDateTime(input, dateTimeFormat)
        );

        // If nothing is returned, user has opted to not commit.
        if (start == null)
        {
            return null;
        }

        // Get the end date time of the CodingSession.
        DateTime? end = UserInputService.GetDateTime(
            $"Enter the end date and time, format [blue]{dateTimeFormat}[/], or [blue]0[/] to return to main menu: ",
            dateTimeFormat,
            input => UserInputValidationService.IsValidCodingSessionEndDateTime(input, dateTimeFormat, start.Value)
            );

        // If nothing is returned, user has opted to not commit.
        if (end == null)
        {
            return null;
        }

        // Start and end contain values, return new CodingSession.
        return new CodingSession(start.Value, end.Value)
        { 
            Id = codingSession.Id
        };
    }

    #endregion
    #region Methods - Private

    private static UserChoice GetOption(List<CodingSession> codingSessions)
    {
        // Add the coding sessions to the existing PageChoices.
        IEnumerable<UserChoice> pageChoices = [.. PageChoices, .. codingSessions.Select(x => new UserChoice(x.Id, $"{x.StartTime.ToString(StringFormat.DateTime)} - {x.EndTime.ToString(StringFormat.DateTime)} ({x.Duration:F2)})"))];

        return AnsiConsole.Prompt(
                new SelectionPrompt<UserChoice>()
                .Title(PromptTitle)
                .AddChoices(pageChoices)
                .UseConverter(c => c.Name!)
                );
    }

    #endregion
}
