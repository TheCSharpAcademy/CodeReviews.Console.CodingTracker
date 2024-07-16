using CodingTracker.ConsoleApp.Models;
using CodingTracker.Constants;
using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker.ConsoleApp.Views;

/// <summary>
/// Page which allows users to select a CodingSession they want to delete.
/// </summary>
internal class DeleteCodingSessionPage : BasePage
{
    #region Constants

    private const string PageTitle = "Delete Coding Session";

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

        return option.Id == 0 ? null : codingSessions.First(x => x.Id == option.Id);
    }

    #endregion
    #region Methods - Private

    private static UserChoice GetOption(List<CodingSession> codingSessions)
    {
        // Add the coding sessions to the existing PageChoices.
        IEnumerable<UserChoice> pageChoices = [.. PageChoices, .. codingSessions.Select(x => new UserChoice(x.Id, $"{x.StartTime.ToString(StringFormat.DateTime)} - {x.EndTime.ToString(StringFormat.DateTime)} ({x.Duration:F2})"))];

        return AnsiConsole.Prompt(
                new SelectionPrompt<UserChoice>()
                .Title(PromptTitle)
                .AddChoices(pageChoices)
                .UseConverter(c => c.Name!)
                );
    }

    #endregion
}
