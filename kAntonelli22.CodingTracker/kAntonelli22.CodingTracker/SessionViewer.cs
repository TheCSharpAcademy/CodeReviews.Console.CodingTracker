using Spectre.Console;

namespace CodingTracker;
internal class SessionViewer
{
    public static void ViewSessions(bool displayOptions, List<CodingSession> sessions)
    {
        Console.Clear();
        var table = new Table();
        table.Border = TableBorder.Horizontal;
        table.Title = new("Coding Sessions:");
        TableColumn[] columns = {new TableColumn(""), new TableColumn("[blue]Start[/]"), new TableColumn("[blue]End[/]"), new TableColumn("[blue]Duration[/]")};
        table.AddColumns(columns);
        for (int i = 0; i < sessions.Count; i++)
            table.AddRow(
            $"{i + 1}.",
            $"{sessions[i].start.ToString("MM/dd/yy hh:mm tt")}",
            $"{sessions[i].end.ToString("MM/dd/yy hh:mm tt")}",
            $"{sessions[i].duration.ToString(@"hh\:mm\:ss")}"
            );
        AnsiConsole.Write(table);

        if (displayOptions)
            ViewingOptions();
    } // end of ViewSessions Method

    public static void ViewingOptions()
    {
        AnsiConsole.WriteLine(@"
    Viewing Options
    ------------------------");
        var menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .AddChoices(new[] {
                "    Exit Coding Tracker", "    Sort By Start Date", "    Sort By End Date",
                "    Sort By Duration", "    Sort By Time Span", "    Return To Menu"
                }));
        AnsiConsole.WriteLine("\n\n\n\n------------------------");

        if (menu == "Exit Coding Tracker")
            Environment.Exit(0);
        else if (menu == "Sort By Start Date")
            SortBy("start");
        else if (menu == "Sort By End Date")
            SortBy("end");
        else if (menu == "Sort By Duration")
            SortBy("duration");
        else if (menu == "Sort By Time Span")
            TimeSpan();
        else if (menu == "Return To Menu")
            return;
        
        ViewSessions(true, CodingSession.sessions);
    } // end of ViewingOptions Method

    public static void SortBy(string sortType)
    {
        bool ascending = AnsiConsole.Confirm("Sort in ascending order?");
        switch (sortType)
        {
            case "start":
                CodingSession.sessions.Sort((x, y) => ascending ? x.start.CompareTo(y.start) : y.start.CompareTo(x.start));
                break;
            case "end":
                CodingSession.sessions.Sort((x, y) => ascending ? x.end.CompareTo(y.end) : y.end.CompareTo(x.end));
                break;
            case "duration":
                CodingSession.sessions.Sort((x, y) => ascending ? x.duration.CompareTo(y.duration) : y.duration.CompareTo(x.duration));
                break;
        }
    } // end of SortBy Method

    public static void TimeSpan()
    {
        Console.WriteLine("What is the start of the time frame? (MM/dd/yy hh:mm tt)");
        DateTime start = InputValidator.GetDate(Console.ReadLine());
        Console.WriteLine("What is the end of the time frame? (MM/dd/yy hh:mm tt)");
        DateTime end = InputValidator.GetDate(Console.ReadLine());

        bool ascending = AnsiConsole.Confirm("Sort in ascending order?");
        List<CodingSession> filteredSessions = CodingSession.sessions
        .Where(session => session.start > start && session.end > end).ToList();

        filteredSessions.Sort((x, y) => ascending ? x.start.CompareTo(y.start) : y.start.CompareTo(x.start));
        ViewSessions(true, filteredSessions);
    } // end of TimeSpan Method
} // end of SessionViewer Class
