using Spectre.Console;

namespace CodingTracker.Mateusz_Platek;

public static class Menu
{
    private static string SelectMenuOption()
    {
        AnsiConsole.Clear();
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[bold underline red]Menu[/]")
            .PageSize(6)
            .MoreChoicesText("[bold red]Move up or down to reveal more options[/]")
            .AddChoices([
                "Exit", "View sessions", "View sorted sessions", "Calculate coding goal",
                "View report", "Add session", "Add session - live", "Update session", "Delete session"
            ])
        );
    }

    public static string SelectSortingMethod()
    {
        Console.Clear();
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold purple]Sorting method[/]")
                .PageSize(3)
                .MoreChoicesText("Move up or down to reveal more options")
                .AddChoices([
                    "[bold red]Ascending[/]", "[bold red]Descending[/]"
                ])
        );
    }

    public static string GetName()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>("[bold red]Insert [yellow]name[/] of the session:[/]")
            );
    }

    public static int GetId()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("[bold red]Insert [yellow]id[/] of the session:[/]")
                .ValidationErrorMessage("[bold red]Input must be a integer[/]")
                .Validate(id =>
                {
                    return id switch
                    {
                        <= 0 => ValidationResult.Error("[bold red]Id must be higher then 0[/]"),
                        _ => ValidationResult.Success()
                    };
                })
        );
    }

    public static DateTime GetDateTime()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<DateTime>("[bold red]Insert date in format [yellow]dd-MM-yyyy HH:mm:SS[/]:[/]")
                .ValidationErrorMessage("[bold red]Incorrect format[/]")
            );
    }

    public static TimeSpan GetTimeSpan()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<TimeSpan>("[bold red]Insert time in format [yellow]HH:mm:SS[/]:[/]")
                .ValidationErrorMessage("[bold red]Incorrect format[/]")
        );
    }

    private static void StopInput()
    {
        AnsiConsole.Prompt(
            new TextPrompt<string>("[bold blue]Press enter to continue[/]")
                .AllowEmpty()
        );
    }
    
    public static void StopSession()
    {
        AnsiConsole.Prompt(
            new TextPrompt<string>("[bold darkorange]Press enter to upload session[/]")
                .AllowEmpty()
        );
    }

    public static void DisplaySessions(List<Session> sessions)
    {
        List<Table> tableSessions = new List<Table>();
        foreach (Session session in sessions)
        {
            Table table = new Table()
                .HideHeaders()
                .AddColumn("")
                .AddColumn("")
                .AddRow("[bold darkorange]Id[/]", $"[bold darkorange]{session.id}[/]")
                .AddRow("[bold purple]Name[/]", $"[bold purple]{session.name}[/]")
                .AddRow("[bold blue]Start[/]", $"[bold blue]{session.start}[/]")
                .AddRow("[bold yellow]End[/]", $"[bold yellow]{session.end}[/]")
                .AddRow("[bold green]Duration[/]", $"[bold green]{session.GetDuration()}[/]");
            tableSessions.Add(table);
        }
        AnsiConsole.Write(new Columns(tableSessions));
    }
    
    public static void Run()
    {
        bool end = false;
        while (!end)
        {
            string input = SelectMenuOption();
            switch (input)
            {
                case "Exit":
                    end = true;
                    break;
                case "View sessions":
                    DatabaseManager.GetSessions();
                    StopInput();
                    break;
                case "View sorted sessions":
                    DatabaseManager.GetSessionsSorted();
                    StopInput();
                    break;
                case "Calculate coding goal":
                    DatabaseManager.CalculateGoal();
                    StopInput();
                    break;
                case "View report":
                    DatabaseManager.GetReport();
                    StopInput();
                    break;
                case "Add session":
                    DatabaseManager.AddSession();
                    StopInput();
                    break;
                case "Add session - live":
                    DatabaseManager.AddSessionLive();
                    StopInput();
                    break;
                case "Update session":
                    DatabaseManager.UpdateSession();
                    StopInput();
                    break;
                case "Delete session":
                    DatabaseManager.DeleteSession();
                    StopInput();
                    break;
            }
        }
    }
}