namespace CodingTracker;
internal class SessionViewer
{
    public static void ViewSessions(bool displayOptions, List<CodingSession> sessions)
    {
        Console.Clear();
        Console.WriteLine("    Coding Sessions:\n            Start         |        End        | Duration");
        for (int i = 0; i < sessions.Count; i++)
            Console.WriteLine($"     {i + 1}. {sessions[i].start.ToString("MM/dd/yy hh:mm tt")} | {sessions[i].end.ToString("MM/dd/yy hh:mm tt")} | {sessions[i].duration.ToString(@"hh\:mm\:ss")}");
        Console.WriteLine($"    -------------------------------------------------------");
        if (displayOptions)
            ViewingOptions();
    } // end of ViewSessions Method

    public static void ViewingOptions()
    {
        Console.WriteLine(@"
    Viewing Options
    ------------------------
    0. Exit Coding Tracker
    1. Sort By Start Date
    2. Sort By End Date
    3. Sort By Duration
    4. Sort By Time Span
    5. Return To Menu
    ------------------------");
        string input = InputValidator.CleanString(Console.ReadLine());
        if (input == "1")
            SortBy("start");
        else if (input == "2")
            SortBy("end");
        else if (input == "3")
            SortBy("duration");
        else if (input == "4")
            TimeSpan();
        else if (input == "5")
            return;
        
        ViewSessions(true, CodingSession.sessions);
    } // end of ViewingOptions Method

    public static void SortBy(string sortType)
    {
        Console.WriteLine("Sort in ascending order? (y/n)");
        string input = InputValidator.CleanString(Console.ReadLine());
        bool ascending = input.Equals("n") ? false : true;

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
        List<CodingSession> filteredSessions = CodingSession.sessions
        .Where(session => session.start > start && session.end > end).ToList();

        ViewSessions(true, filteredSessions);
    } // end of TimeSpan Method
} // end of SessionViewer Class
