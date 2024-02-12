using Spectre.Console;

namespace CodingTracker.Mateusz_Platek;

public static class DatabaseManager
{
    public static void GetSessions()
    {
        List<Session> sessions = Database.GetSessions();
        
        AnsiConsole.Write(new Markup("[bold underline green]Your coding sessions:[/]\n"));
        Menu.DisplaySessions(sessions);
    }

    public static void GetSessionsSorted()
    {
        List<Session> sessions = Database.GetSessions();

        string? input = Menu.SelectSortingMethod();
        switch (input)
        {
            case "Ascending":
                sessions.Sort((o1, o2) => o1.GetDuration().CompareTo(o2.GetDuration()));
                break;
            case "Descending":
                sessions.Sort((o1, o2) => o2.GetDuration().CompareTo(o1.GetDuration()));
                break;
        }
        
        AnsiConsole.Write(new Markup("[bold underline green]Your coding sessions:[/]\n"));
        Menu.DisplaySessions(sessions);
    }

    public static void GetReport()
    {
        TimeSpan totalTimeSpan = new TimeSpan(0);
        
        List<Session> sessions = Database.GetSessions();
        foreach (Session session in sessions)
        {
            totalTimeSpan += session.GetDuration();
        }

        TimeSpan averageTimeSpan = totalTimeSpan / sessions.Count;

        Table table = new Table()
            .Title("[bold underline purple]Coding report[/]")
            .HideHeaders()
            .AddColumn("")
            .AddColumn("")
            .AddRow("[bold lime]Total coding time[/]", $"[bold lime]{totalTimeSpan}[/]")
            .AddRow("[bold red]Average coding time[/]", $"[bold red]{averageTimeSpan}[/]");
        AnsiConsole.Write(table);
    }

    public static void CalculateGoal()
    {
        TimeSpan goalTimeSpan = Menu.GetTimeSpan();
        
        List<Session> sessions = Database.GetSessions();
        sessions.RemoveAll(session => session.start.Month != DateTime.Now.Month);
        
        TimeSpan currentTimeSpan = new TimeSpan();
        foreach (Session session in sessions)
        {
            currentTimeSpan += session.GetDuration();
        }
        
        DateTime dateTimeNow = DateTime.Now;
        int daysInMonth = DateTime.DaysInMonth(dateTimeNow.Year, dateTimeNow.Month);
        int daysToEndOfTheMonth = daysInMonth - dateTimeNow.Day;

        TimeSpan missingTimeSpan = goalTimeSpan - currentTimeSpan;
        TimeSpan codingPerDay = (goalTimeSpan - currentTimeSpan) / daysToEndOfTheMonth;

        Table table = new Table()
            .Title("[bold underline yellow]Coding goal[/]")
            .HideHeaders()
            .AddColumn("")
            .AddColumn("")
            .AddRow("[bold orchid]Goal coding time[/]", $"[bold orchid]{goalTimeSpan}[/]")
            .AddRow("[bold blue]Current coding time[/]", $"[bold blue]{currentTimeSpan}[/]")
            .AddRow("[bold hotpink]Missing coding time[/]", $"[bold hotpink]{missingTimeSpan}[/]")
            .AddRow("[bold tan]Coding time per day[/]", $"[bold tan]{codingPerDay}[/]");
        AnsiConsole.Write(table);
    }

    public static void AddSession()
    {
        string name = Menu.GetName();
        DateTime dateTimeStart = Menu.GetDateTime();
        DateTime dateTimeEnd = Menu.GetDateTime();
        if (dateTimeStart > dateTimeEnd)
        {
            AnsiConsole.Write(new Markup("[bold red]Incorrect dates[/]\n"));
            return;
        }
        
        Database.AddSession(new Session(name, dateTimeStart, dateTimeEnd));
        AnsiConsole.Write(new Markup("[bold yellow]Session added[/]\n"));
    }

    public static void AddSessionLive()
    {
        string name = Menu.GetName();
        
        DateTime dateTimeStart = DateTime.Now;
        AnsiConsole.Write(new Markup($"[bold darkcyan]Start of the session: [purple]{dateTimeStart}[/][/]\n"));
        
        Menu.StopSession();
        DateTime dateTimeEnd = DateTime.Now;
        AnsiConsole.Write(new Markup($"[bold darkcyan]End of the session: [purple]{dateTimeEnd}[/][/]\n"));

        TimeSpan duration = dateTimeEnd - dateTimeStart;
        AnsiConsole.Write(new Markup($"[bold darkcyan]Duration: [purple]{duration}[/][/]\n"));
        
        Database.AddSession(new Session(name, dateTimeStart, dateTimeEnd));
        
        AnsiConsole.Write(new Markup("[bold yellow]Session added[/]\n"));
    }

    public static void UpdateSession()
    {
        GetSessions();
        
        int id = Menu.GetId();
        string name = Menu.GetName();
        DateTime dateTimeStart = Menu.GetDateTime();
        DateTime dateTimeEnd = Menu.GetDateTime();
        
        Database.UpdateSession(new Session(id, name, dateTimeStart, dateTimeEnd));
        
        AnsiConsole.Write(new Markup("[bold yellow]Session updated[/]\n"));
    }

    public static void DeleteSession()
    {
        GetSessions();
        
        int id = Menu.GetId();
        
        Database.DeleteSession(id);
        
        AnsiConsole.Write(new Markup("[bold yellow]Session deleted[/]\n"));
    }
}