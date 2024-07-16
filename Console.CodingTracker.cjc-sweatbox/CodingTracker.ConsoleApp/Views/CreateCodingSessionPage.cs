using CodingTracker.ConsoleApp.Services;
using CodingTracker.Constants;
using CodingTracker.Models;
using CodingTracker.Services;
using Spectre.Console;

namespace CodingTracker.ConsoleApp.Views;

/// <summary>
/// Page which allows users to input a start and end date to create a CodingSession.
/// </summary>
internal class CreateCodingSessionPage : BasePage
{
    #region Constants

    private const string PageTitle = "Create Coding Session";

    #endregion
    #region Methods

    internal static CodingSession? Show()
    {
        AnsiConsole.Clear();

        WriteHeader(PageTitle);

        // What date and time string format the coding session uses.
        string dateTimeFormat = StringFormat.DateTime;

        // Get the start date time of the CodingSession.
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
        return new CodingSession(start.Value, end.Value);
    }

    #endregion
}
