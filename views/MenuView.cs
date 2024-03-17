using CodingTracker.enums;
using CodingTracker.utils;
using Spectre.Console;

namespace CodingTracker.views;

/// <summary>
/// Provides static methods for displaying menu options to the user.
/// </summary>
public static class MenuView
{
    internal static MainMenuEntries ShowMainMenu()
    {
        var mainMenuEntries = Utilities.GetEnumValuesAndDisplayNames<MainMenuEntries>();

        return ShowMenuPrompt(mainMenuEntries);
    }

    internal static ReportTypes ShowReportsMenu()
    {
        var reportMenuEntries = Utilities.GetEnumValuesAndDisplayNames<ReportTypes>();
        
        return ShowMenuPrompt(reportMenuEntries);
    }

    internal static TimerMenuEntries ShowTimerMenu()
    {
        var timerMenuEntries = Utilities.GetEnumValuesAndDisplayNames<TimerMenuEntries>();
        
        return ShowMenuPrompt(timerMenuEntries);
    }

    /// <summary>
    /// Prompts the user to select an entry from the menu and returns the selected entry.
    /// </summary>
    /// <typeparam name="TEnum">The enum type of menu entries.</typeparam>
    /// <param name="menuEntries">The collection of menu entries with their display names.</param>
    /// <returns>The selected menu entry or null if there are no menu entries or an error occurred.</returns>
    private static TEnum ShowMenuPrompt<TEnum>(
        IEnumerable<KeyValuePair<TEnum, string>> menuEntries) where TEnum : struct, Enum
    {
        Console.Clear();
        
        if (!menuEntries.Any())
        {
            AnsiConsole.WriteLine($"Problem reading {menuEntries.GetType().Name} entries.");
            return default;
        }
        
        var userChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What would you like to do?\n\n'0' (zero) on any prompt to get here.")
                .AddChoices(menuEntries.Select(e => e.Value))
            );
        
        var selectedEntry = menuEntries.SingleOrDefault(e => e.Value == userChoice);

        return selectedEntry.Key;
    }
}