using System.Diagnostics;

namespace Ohshie.CodingTracker.SessionsOperator;

public class NewCodingSession
{
    private readonly string _time = DateTime.Now.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
    private readonly Stopwatch _stopwatch = new();

    public void NewSession()
    {
        string sessionDescription = AddDescriptionToSession();
        
        Session newSession = TrackSession(sessionDescription);
        
        newSession.Note = AddNoteToSession(newSession);

        DbOperations.DbOperations dbOperations = new();
        dbOperations.NewSessionEntry(newSession);
    }

    private string AddDescriptionToSession()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("Great! You are ready to start new session"));
        string description = AnsiConsole.Ask<string>("Do you plan anything special about this session?\n" +
                                                     "type it here: ");
        return description;
    }

    private string AddNoteToSession(Session session)
    {
        AnsiConsole.Write(new Rule("Session ended"));
        AnsiConsole.Write(new Markup($"Session lasted for [bold]{session.Length}[/]. [bold]Nice![/]\n"));
        
        string note = AnsiConsole.Ask<string>("Any notes about this one?\n Type them here:");
        return note;
    }

    private Session TrackSession(string sessionDescription)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("Session in progress"));
        
        Session newSession = new()
        {
            Description = sessionDescription,
            Date = _time
        };
        
        _stopwatch.Start();
        ConsoleProgress();
        _stopwatch.Stop();

        newSession.Length = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
        
        return newSession;
    }

    private void ConsoleProgress()
    {
        AnsiConsole.Progress()
            .AutoRefresh(true)
            .Columns(new ProgressColumn[]
            {
                new TaskDescriptionColumn(),
                new ElapsedTimeColumn(),
                new SpinnerColumn()
            }).Start(ctx =>
            {
                AnsiConsole.WriteLine($"Session in progress. You started at {_time}.");
                var task = ctx.AddTask(description: "You've been coding for:");
                AnsiConsole.WriteLine("Press enter when finished.");

                while (!task.IsFinished)
                {
                    if (string.IsNullOrEmpty(Console.ReadLine())) task.StopTask();

                    task.Increment(1);
                }
            });
    }
}