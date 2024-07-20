using Spectre.Console;

namespace CodingTracker;
internal class SessionViewer
{
    public static void ViewSessions(bool displayOptions, List<CodingSession> sessions)
    {
        Console.Clear();
        var table = new Table();
        table.Border = TableBorder.Markdown;
        table.Title = new("Coding Sessions:");

        TableColumn[] columns = {new TableColumn("[blue]Start[/]"), new TableColumn("[blue]End[/]"), new TableColumn("[blue]Duration[/]")};
        table.AddColumns(columns);
        for (int i = 0; i < sessions.Count; i++)
            table.AddRow(
            $"{sessions[i].start.ToString("MM/dd/yy hh:mm tt")}",
            $"{sessions[i].end.ToString("MM/dd/yy hh:mm tt")}",
            $"{sessions[i].duration.ToString(@"hh\:mm\:ss")}"
            );
        AnsiConsole.Write(table);

        if (displayOptions)
            ViewingOptions(sessions);
    } // end of ViewSessions Method

    public static void ViewingOptions(List<CodingSession> sessions)
    {
        AnsiConsole.WriteLine("Viewing Options\n------------------------");
        var menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .AddChoices(new[] {
                "Exit Coding Tracker", "Sort By Date", "Sort By Duration", 
                "Sort By Time Span", "Session Stats", "<-- Back"
                }));

        if (menu == "Exit Coding Tracker")
            Environment.Exit(0);
        else if (menu == "Sort By Date")
            SortBy("date");
        else if (menu == "Sort By Duration")
            SortBy("duration");
        else if (menu == "Sort By Time Span")
            SortByTimeSpan();
        else if (menu == "Session Stats")
            DisplayStats(sessions);
        else if (menu == "<-- Back")
            return;
        
        ViewSessions(true, sessions);
    } // end of ViewingOptions Method

    public static void SortBy(string sortType)
    {
        bool ascending = AnsiConsole.Confirm("Sort in ascending order?");

        if (sortType == "date")
            CodingSession.sessions.Sort((x, y) => ascending ? x.start.CompareTo(y.start) : y.start.CompareTo(x.start));
        else if (sortType == "duration")
            CodingSession.sessions.Sort((x, y) => ascending ? x.duration.CompareTo(y.duration) : y.duration.CompareTo(x.duration));
    } // end of SortBy Method

    public static void SortByTimeSpan()
    {
        Console.WriteLine("What is the start of the time frame? (MM/dd/yy hh:mm tt)");
        DateTime start = InputValidator.GetDate(Console.ReadLine());
        Console.WriteLine("What is the end of the time frame? (MM/dd/yy hh:mm tt)");
        DateTime end = InputValidator.GetDate(Console.ReadLine());

        bool ascending = AnsiConsole.Confirm("Sort in ascending order?");
        List<CodingSession> filteredSessions = CodingSession.sessions
        .Where(session => session.start > start && session.end < end).ToList();

        filteredSessions.Sort((x, y) => ascending ? x.start.CompareTo(y.start) : y.start.CompareTo(x.start));
        ViewSessions(true, filteredSessions);
    } // end of TimeSpan Method

    public static void DisplayStats(List<CodingSession> sessions)
    {
        TimeSpan totalHours = new();
        foreach (var session in sessions)
            totalHours += session.duration;
        TimeSpan avgHours = totalHours / sessions.Count;
        AnsiConsole.MarkupLineInterpolated($"Total hours spent coding: [blue]{totalHours.ToString(@"hh\:mm\:ss")}[/].");
        AnsiConsole.MarkupLineInterpolated($"Average hours per session: [blue]{avgHours.ToString(@"hh\:mm\:ss")}[/].");
        Output.ReturnToMenu("");
    }
} // end of SessionViewer Class
