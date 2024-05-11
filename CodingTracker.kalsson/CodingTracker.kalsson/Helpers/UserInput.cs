using Spectre.Console;

namespace CodingTracker.kalsson;

public static class UserInput
{
    /// <summary>
    /// Retrieves user input from the console.
    /// </summary>
    /// <param name="prompt">The prompt displayed to the user.</param>
    /// <param name="cancelPrompt">The cancel prompt displayed to the user (optional).</param>
    /// <returns>The user input, or null if "cancel" is entered.</returns>
    public static string GetInput(string prompt, string cancelPrompt = "or type [red]'cancel'[/] to return:")
    {
        var input = AnsiConsole.Ask<string>($"{prompt} {cancelPrompt}");
        if (input.Equals("cancel", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
        return input;
    }

    /// <summary>
    /// Retrieves the user input as a <see cref="DateTime"/> object from the console.
    /// </summary>
    /// <param name="prompt">The prompt displayed to the user.</param>
    /// <returns>The user input as a <see cref="DateTime"/> object, or null if "cancel" is entered.</returns>
    public static DateTime? GetDateTime(string prompt)
    {
        while (true)
        {
            var input = GetInput($"{prompt} (yyyy-mm-dd hh:mm)", "or type [red]'cancel'[/] to return. Use 'yyyy-mm-dd hh:mm' format:");
            if (input == null) return null;

            if (DateTime.TryParse(input, out DateTime dateTime))
            {
                return dateTime;
            }

            AnsiConsole.MarkupLine("[red]Invalid date format. Please try again.[/]");
        }
    }

    /// <summary>
    /// Asks the user to confirm an action.
    /// </summary>
    /// <param name="message">The message displayed to the user.</param>
    /// <returns>True if the user confirms the action, otherwise false.</returns>
    public static bool ConfirmAction(string message)
    {
        return AnsiConsole.Confirm(message);
    }
}