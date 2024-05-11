using Spectre.Console;

namespace CodingTracker.kalsson;

public static class UserInput
{
    public static string GetInput(string prompt, string cancelPrompt = "or type [red]'cancel'[/] to return:")
    {
        var input = AnsiConsole.Ask<string>($"{prompt} {cancelPrompt}");
        if (input.Equals("cancel", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
        return input;
    }

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
    
    public static bool ConfirmAction(string message)
    {
        return AnsiConsole.Confirm(message);
    }
}