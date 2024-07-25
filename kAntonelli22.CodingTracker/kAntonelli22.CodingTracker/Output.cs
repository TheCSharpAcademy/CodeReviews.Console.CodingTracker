using System.Diagnostics;
using CodingTracker.DatabaseUtilities;
using Spectre.Console;

namespace CodingTracker;
internal class Output
{
    public static Stopwatch Stopwatch  { get; set; } = new Stopwatch();
    public static bool StopwatchRunning  { get; set; } = false;
    public static void StartTimed()
    {
        Stopwatch.Start();
        StopwatchRunning = true;
        Console.Clear();
        ReturnToMenu("Starting new Timed Session");

    } // end of StartSession Method
    
    public static void EndTimed()
    {
        Console.Clear();
        DateTime end = DateTime.Now;
        DateTime start = end - Stopwatch.Elapsed;
        Stopwatch.Reset();
        StopwatchRunning = false;

        CodingSession newSession = new(start, end);
        DatabaseManager.InsertSession(newSession);
        AnsiConsole.MarkupLineInterpolated($"Session Logged. Session duration: [blue]{newSession.Duration.TotalHours} hours[/]. ");
        ReturnToMenu("");
    } // end of EndSession Method
    
    public static void NewSession()
    {
        Console.Clear();
        Console.WriteLine("When did the session start? (MM/dd/yy hh:mm tt)");
        DateTime start = InputValidator.GetDate();
        Console.WriteLine("When did the session end? (MM/dd/yy hh:mm tt)");
        DateTime end = InputValidator.GetDate();

        CodingSession newSession = new(start, end);
        DatabaseManager.InsertSession(newSession);

        Console.Clear();
        SessionViewer.ViewSessions(false, CodingSession.sessions);
        ReturnToMenu("Creating new session");
    } // end of NewSession Method
    
    public static void ModifySession()
    {
        Console.Clear();
        SessionViewer.ViewSessions(false, CodingSession.sessions);
        Console.WriteLine("Which Session would you like to modify. Input the ID?");
        int sessionNumber = InputValidator.CleanInt();

        Console.WriteLine("When did the session start? (MM/dd/yy hh:mm tt)");
        DateTime start = InputValidator.GetDate();
        Console.WriteLine("When did the session end? (MM/dd/yy hh:mm tt)");
        DateTime end = InputValidator.GetDate();

        var session = CodingSession.sessions[sessionNumber - 1];
        session.Start = start;
        session.End = end;
        session.Duration = end - start;

        int rowid = DatabaseManager.GetID(sessionNumber - 1);
        string query = $"UPDATE Sessions SET start = '{start}', end = '{end}', duration = '{end - start}' WHERE Id = {rowid}";
        DatabaseManager.RunQuery(query);

        Console.Clear();
        SessionViewer.ViewSessions(false, CodingSession.sessions);
        ReturnToMenu("Session Modified");
    } // end of ModifySession Method
    
    public static void RemoveSession()
    {
        Console.Clear();
        SessionViewer.ViewSessions(false, CodingSession.sessions);
        Console.WriteLine("Which Session would you like to remove?");
        int sessionNumber = InputValidator.CleanInt();
        
        int rowid = DatabaseManager.GetID(sessionNumber - 1);
        string query = $"DELETE FROM Sessions WHERE Id = {rowid}";
        DatabaseManager.RunQuery(query);
        CodingSession.sessions.Remove(CodingSession.sessions[sessionNumber - 1]);

        Console.Clear();
        SessionViewer.ViewSessions(false, CodingSession.sessions);
        ReturnToMenu("Session Removed");
    } // end of RemoveSession Method
    
    public static void ReturnToMenu(string message)
    {
        if (message == "")
            Console.WriteLine($"Press enter to return to menu.");
        else
            Console.WriteLine($"{message}. Press enter to return to menu.");
        InputValidator.CleanString();
    } // end of ReturnToMenu Method
} // end of UserInput Class
