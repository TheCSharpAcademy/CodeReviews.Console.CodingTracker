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

        TableColumn[] columns = [new("[blue]ID[/]"), new("[blue]Start[/]"), new("[blue]End[/]"), new("[blue]Duration[/]")];
        table.AddColumns(columns);
        for (int i = 0; i < sessions.Count; i++)
            table.AddRow(
            $"{i + 1}",
            $"{sessions[i].Start:MM/dd/yy hh:mm tt}",
            $"{sessions[i].End:MM/dd/yy hh:mm tt}",
            $"{sessions[i].Duration:hh\\:mm\\:ss}"
            );
        AnsiConsole.Write(table);

        if (displayOptions)
            ViewingOptions(sessions);
    } // End of ViewSessions Method

    public static void ViewingOptions(List<CodingSession> sessions)
    {
        AnsiConsole.WriteLine("Viewing Options\n------------------------");
        var menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .AddChoices([
                "Exit Coding Tracker", "Sort By Date", "Sort By Duration", 
                "Sort By Time Span", "Session Stats", "<-- Back"
                ]));

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
    } // End of ViewingOptions Method

    public static void SortBy(string sortType)
    {
        bool ascEnding = AnsiConsole.Confirm("Sort in ascending order?");

        if (sortType == "date")
            CodingSession.Sessions.Sort((x, y) => ascEnding ? x.Start.CompareTo(y.Start) : y.Start.CompareTo(x.Start));
        else if (sortType == "duration")
            CodingSession.Sessions.Sort((x, y) => ascEnding ? x.Duration.CompareTo(y.Duration) : y.Duration.CompareTo(x.Duration));
    } // End of SortBy Method

    public static void SortByTimeSpan()
    {
        Console.WriteLine("What is the Start of the time frame? (MM/dd/yy hh:mm tt)");
        DateTime Start = InputValidator.GetDate();
        Console.WriteLine("What is the End of the time frame? (MM/dd/yy hh:mm tt)");
        DateTime End = InputValidator.GetDate();

        bool ascending = AnsiConsole.Confirm("Sort in ascending order?");
        List<CodingSession> filteredSessions = CodingSession.Sessions
        .Where(session => session.Start > Start && session.End < End).ToList();

        filteredSessions.Sort((x, y) => ascending ? x.Start.CompareTo(y.Start) : y.Start.CompareTo(x.Start));
        ViewSessions(true, filteredSessions);
    } // End of TimeSpan Method

    public static void DisplayStats(List<CodingSession> sessions)
    {
        TimeSpan totalHours = new();
        foreach (var session in sessions)
            totalHours += session.Duration;
        TimeSpan avgHours = totalHours / sessions.Count;
        AnsiConsole.MarkupLineInterpolated($"Total hours spent coding: [blue]{totalHours.TotalHours} hours[/].");
        AnsiConsole.MarkupLineInterpolated($"Average hours per session: [blue]{avgHours:hh\\:mm\\:ss}[/].");
        Output.ReturnToMenu("");
    }
} // End of SessionViewer Class
