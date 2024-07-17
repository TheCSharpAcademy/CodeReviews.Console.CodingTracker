using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker;
internal class Output
{
    public static Stopwatch stopwatch = new Stopwatch();
    public static bool stopwatchRunning = false;
    public static void StartTimed()
    {
        stopwatch.Start();
        stopwatchRunning = true;
        Console.Clear();
        ReturnToMenu("Starting new Timed Session");

    } // end of StartSession Method
    
    public static void EndTimed()
    {
        Console.Clear();
        DateTime end = DateTime.Now;
        DateTime start = end - stopwatch.Elapsed;
        stopwatch.Reset();
        stopwatchRunning = false;
        CodingSession newSession = new(start, end);
        // insert session into Coding table
        Console.WriteLine($"Session Logged. Session duration: {newSession.duration.ToString("g")}. ");
        ReturnToMenu("");
    } // end of EndSession Method
    
    public static void NewSession()
    {
        Console.Clear();
        Console.WriteLine("When did the session start?");
        DateTime start = UserInput.GetDate(Console.ReadLine());
        Console.WriteLine("When did the session end?");
        DateTime end = UserInput.GetDate(Console.ReadLine());

        CodingSession newSession = new(start, end);
        ReturnToMenu("Creating new session");
    } // end of NewSession Method
    
    public static void ModifySession()
    {
        Console.Clear();
        ViewSessions(false);
        Console.WriteLine("Which Session would you like to modify?");
        int sessionNumber = UserInput.CleanInt(Console.ReadLine());

        Console.WriteLine("When did the session start?");
        DateTime start = UserInput.GetDate(Console.ReadLine());
        Console.WriteLine("When did the session end?");
        DateTime end = UserInput.GetDate(Console.ReadLine());

        var session = CodingSession.sessions[sessionNumber - 1];
        session.start = start;
        session.end = end;
        session.duration = end - start;

        ReturnToMenu("Session Modified");
    } // end of ModifySession Method
    
    public static void RemoveSession()
    {
        Console.Clear();
        ViewSessions(false);
        Console.WriteLine("Which Session would you like to remove?");
        int sessionNumber = UserInput.CleanInt(Console.ReadLine());
        CodingSession.sessions.Remove(CodingSession.sessions[sessionNumber - 1]);
        ReturnToMenu("Session Removed");
    } // end of RemoveSession Method
    
    public static void ViewSessions(bool calledByMenu)
    {
        Console.Clear();
        Console.WriteLine("    Coding Sessions:");
        for (int i = 0; i < CodingSession.sessions.Count; i++)
            Console.WriteLine($"     {i + 1}. {CodingSession.sessions[i].start.ToString("g")} | {CodingSession.sessions[i].end.ToString("g")} | {CodingSession.sessions[i].duration.ToString("G")}");
        Console.WriteLine($"    -------------------------------------------------------");
        if (calledByMenu)
            ReturnToMenu("");
    } // end of ViewSessions Method
    public static void ReturnToMenu(string message)
    {
        if (message == "")
            Console.WriteLine($"Press enter to return to menu.");
        else
            Console.WriteLine($"{message}. Press enter to return to menu.");
        Console.ReadLine();
    } // end of ReturnToMenu Method
} // end of UserInput Class
