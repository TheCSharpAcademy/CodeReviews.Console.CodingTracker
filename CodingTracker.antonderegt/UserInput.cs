using Spectre.Console;

namespace CodingTracker;

public class UserInput
{
    public static string PromptUserAction()
    {
        AnsiConsole.Clear();

        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What do you want to [green]do[/]?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                .AddChoices([
                    ActionType.StartSession,
                    ActionType.EndSession,
                    ActionType.EditSession,
                    ActionType.DeleteSession,
                    ActionType.ShowSessions,
                    ActionType.ExitProgram
                ])
        );

        return option;
    }

    public static DateTime PromptDateTime(string dateType)
    {
        AnsiConsole.Clear();

        string promptedTime = AnsiConsole.Prompt(
            new TextPrompt<string>($"[green]Enter {dateType} time (yyyy-mm-dd hh:mm)[/] Or leave empty for current time: ")
                .AllowEmpty()
        );

        if (promptedTime.Equals(string.Empty))
        {
            return DateTime.Now;
        }

        if (DateTime.TryParse(promptedTime, out DateTime dateTime))
        {
            return dateTime;
        }

        AnsiConsole.Markup("[red]Failed to read date[/]. Press enter to try again...");
        Console.ReadLine();
        return PromptDateTime(dateType);
    }

    public static int PromptSessionId()
    {
        string promptedId = AnsiConsole.Prompt(
            new TextPrompt<string>("[green]Enter id. [/] Or leave empty to cancel: ")
            .AllowEmpty()
        );

        if (int.TryParse(promptedId, out int id))
        {
            return id;
        }

        return -1;
    }
}