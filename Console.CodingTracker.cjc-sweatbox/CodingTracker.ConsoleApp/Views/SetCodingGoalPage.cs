using CodingTracker.ConsoleApp.Services;
using CodingTracker.Models;
using CodingTracker.Services;
using Spectre.Console;

namespace CodingTracker.ConsoleApp.Views;

/// <summary>
/// Page which allows users to set a CodingGoal.
/// </summary>
internal class SetCodingGoalPage : BasePage
{
    #region Constants

    private const string PageTitle = "Set Coding Goal";
    
    #endregion
    #region Methods: Internal

    internal static CodingGoal Show()
    {
        AnsiConsole.Clear();

        WriteHeader(PageTitle);

        double duration = UserInputService.GetDouble(
            $"Enter your weekly coding goal duration in hours: ",
            input => UserInputValidationService.IsValidCodingGoalDuration(input)
            );

        return new CodingGoal(duration);
    }

    #endregion
}
