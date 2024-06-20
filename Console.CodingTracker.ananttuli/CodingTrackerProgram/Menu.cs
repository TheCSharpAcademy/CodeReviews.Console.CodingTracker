using Spectre.Console;

namespace CodingTrackerProgram;

public class Menu
{
    public const string CREATE_SESSION = "Create coding session log";
    public const string VIEW_SESSIONS = "View coding sessions";
    public const string UPDATE_SESSION = "Update coding session log";
    public const string DELETE_SESSION = "Delete coding session log";
    public const string EXIT_PROGRAM = "[red]Exit[/]";

    public static string ShowMenu()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .EnableSearch()
            .Title("\n\t[green]Menu[/]")
            .AddChoices(
                CREATE_SESSION,
                VIEW_SESSIONS,
                UPDATE_SESSION,
                DELETE_SESSION,
                EXIT_PROGRAM
            )
        );
    }
}