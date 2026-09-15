using System.Formats.Asn1;
using Spectre.Console;
public class Menu
{
    private readonly DataAccess _db;
    public Menu(DataAccess db)
    {
        _db = db;
    }
    public void MainMenu()
    {
        
        while (true)
        {
            Console.Clear();
            // AnsiConsole.MarkupLine("[green] Coding Tracker[/]").Centered();

            AnsiConsole.Write(
                new Align(
                    new Markup("[green]Coding Tracker[/]"),
                    HorizontalAlignment.Center
                )
            );


            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select an option: ")
                .AddChoices("Add a Session", "Delete a Session", "View All Sessions",
                "Start a Session with a Timer")
            );

            switch (choice)
            {
                case "Add a Session":
                    AddSessionMenu();
                    break;

                case "Delete a Session":
                    DeleteSessionMenu();
                    break;

                case "View All Sessions":
                    ViewSessionsMenu();
                    break;

                case "Start a Session with a Timer":
                    StartSessionMenu();
                    break;

                default:
                    AnsiConsole.MarkupLine("[yellow] Please choose an option[/]");
                    continue;
            }
        }
    }

    public void AddSessionMenu()
    {
        Console.Clear();
        while (true)
        {
            DateTime startTime;
            DateTime endTime;

            AnsiConsole.MarkupLine("Enter Dates and Times in this format [green]yyyy-MM-dd HH:mm:ss[/]");
            AnsiConsole.MarkupLine("[yellow] Or leave empty to enter the current datetime! [/]");
            
            while (true)
                {
                    string input = AnsiConsole.Prompt(
                        new TextPrompt<string>("Enter the start time:")
                            .AllowEmpty()
                    );

                    if (DateTimeHelper.TryGetDateTime(input, out startTime))
                        break;

                    AnsiConsole.MarkupLine(
                        "[red]Invalid input, please try again.[/]"
                    );
                }

            while (true)
                {
                    string input = AnsiConsole.Prompt(
                        new TextPrompt<string>("Enter the end time:")
                            .AllowEmpty()
                    );

                    if (DateTimeHelper.TryGetDateTime(input, out endTime))
                        break;

                    AnsiConsole.MarkupLine(
                        "[red]Invalid input, please try again.[/]"
                    );
                }
            
            TimeSpan duration = endTime - startTime;
            string description = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter a Description or a Note [yellow](optional)[/]")
                    .AllowEmpty()
            );

            if (AnsiConsole.Confirm("Add Session?"))
            {
                _db.CreateSession(new Session { StartTime = startTime, EndTime = endTime,
                    Duration = duration, Description = description});

                AnsiConsole.MarkupLine("[green]Session Added[/]");

                break;
            } else
            {
                break;
            }
        }
        

            
    }

    public void DeleteSessionMenu()
    {
        Console.Clear();

        List<Session> sessions = _db.ReadSessions();
        AnsiConsole.MarkupLine("Enter the [blue]ID[/] of the session you would like to delete:");
        DisplayTable(sessions);

        while (true)
        {    
            int IdToDelete = AnsiConsole.Ask<int>("ID:");

            
            if (sessions.Any(s => s.Id == IdToDelete)
                && AnsiConsole.Confirm("Delete session?") 
                )
            {
                _db.DeleteSession(IdToDelete);
                AnsiConsole.MarkupLine("[green]Session Deleted[/]");
                break;
            } else
            {
                AnsiConsole.MarkupLine("[red]Session does not exist, please try again.[/]");
                continue;
            }
        }
        
    }

    public void ViewSessionsMenu()
    {
        Console.Clear();

        List<Session> sessions = _db.ReadSessions();
        DisplayTable(sessions);
        AnsiConsole.MarkupLine("Press [blue]ESC[/] to go back");
        
        while (Console.ReadKey(true).Key != ConsoleKey.Escape)
        {
            
        }
    }
    
    public void StartSessionMenu()
    {
        Console.Clear();
        SessionService stopWatchData = new SessionService();
        stopWatchData.StartSession();
      while (true)
        {
            Console.Clear();
            var stopWatch = new Panel(
                new Rows(
                    new Markup(" "),
                    new Markup($"[bold green]{stopWatchData.GetElapsedTime()
                                    .ToString(@"hh\:mm\:ss")}[/]").Centered(),
                    new Markup(" "),
                    new Markup("[yellow]Press ESC to stop[/]").Centered()
                )
            )
                .Header("[bold blue] Live Session [/]", Justify.Center)
                .DoubleBorder()
                .Expand();

            AnsiConsole.Write(stopWatch);

             if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Escape)
                    {
                        if (AnsiConsole.Confirm("[bold] Add this session? [/]"))
                        {
                            string description = AnsiConsole.Prompt(
                                new TextPrompt<string>("Enter a Description or a Note [yellow](optional)[/]")
                                    .AllowEmpty()
                            );
                            Session s = stopWatchData.EndSession();
                            s.Description = description;
                            _db.CreateSession(s);
                            AnsiConsole.MarkupLine("[bold green] Session Created![/]");
                        }
                        Console.Clear();
                        break;
                    }
                }

    Thread.Sleep(1000);

        }  
    }
    public void DisplayTable(List<Session> list)
    {
        var table = new Table()
            .Title("[green]Session List[/]");
        
        table.AddColumn("ID")
            .AddColumn("Start Time")
            .AddColumn("End Time")
            .AddColumn("Duration")
            .AddColumn("Description");

        foreach (var v in list)
        {
            table.AddRow(
                v.Id.ToString(),
                v.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                v.EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
                v.Duration.ToString(@"hh\:mm\:ss"),
                v.Description ?? ""
            );
        }
        AnsiConsole.Write(table);
    }
}