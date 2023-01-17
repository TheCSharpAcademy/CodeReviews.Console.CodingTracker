using CodingTracker;
using System.Diagnostics;

internal static class UserController
{
    private static readonly DataAccessor dal = new();
    private static readonly Stopwatch stopwatch = new();
    private static DateTime stopwatchStartTime = DateTime.MinValue;
    private static DateTime stopwatchEndTime = DateTime.MinValue;

    public static void InitializeDatabase()
    {
        DataAccessor dal = new();
        dal.InitializeDatabase();
    }

    public static void ProcessInput(string userInput)
    {
        switch (userInput)
        {
            case "v":
                ViewTableFiltered();
                break;
            case "a":
                AddEntry();
                break;
            case "d":
                DeleteEntry();
                break;
            case "u":
                UpdateEntry();
                break;
            case "srt":
                StartStopwatch();
                break;
            case "stp":
                StopStopwatch();
                break;
            case "0":
                Program.SetEndAppToTrue();
                break;
            default:
                break;
        }
    }

    private static void StartStopwatch()
    {
        Console.WriteLine("\nWe've started a stopwatch - get coding!");
        stopwatch.Start();
        stopwatchStartTime = DateTime.Now;
    }

    private static void StopStopwatch()
    {
        if (stopwatch != null)
        {
            stopwatch.Stop();
            stopwatchEndTime = DateTime.Now;
            TimeSpan duration = stopwatch.Elapsed;
            dal.AddEntry(Validator.ConvertFromDate(stopwatchStartTime), Validator.ConvertFromDate(stopwatchEndTime), duration.ToString());
            Console.WriteLine("\nStopwatch time recorded");
        }
        else
        {
            Console.WriteLine("\nNo stopwatch is running.");
        }
    }

    private static void ViewTableFiltered()
    {
        Viewer.DisplayFilterOptionsMenu();
        string filter = UserInput.GetUserFilterChoice();
        List<CodingSession> sessions = dal.GetCodingSessions();
        sessions = Viewer.FilterSessions(sessions, filter);
        Viewer.ViewTable(sessions);
    }

    private static void AddEntry()
    {
        Viewer.DisplayPromptForTime("start");
        string startTime = UserInput.GetStartTime();

        Viewer.DisplayPromptForTime("finish");
        string endTime = UserInput.GetEndTime(Validator.ConvertToDate(startTime));

        TimeSpan duration = Validator.CalculateDuration(startTime, endTime);
        dal.AddEntry(startTime, endTime, duration.ToString());
    }

    private static void DeleteEntry()
    {
        ViewTableFiltered();

        Console.Write($"\nWhich entry do you want to remove? ");
        int id = UserInput.GetIdForUpdate();
        if (id == -1) { return; }

        dal.DeleteEntry(id);
        Console.WriteLine("\nSuccessfully delteded entry " + id);
    }

    private static void UpdateEntry()
    {
        Viewer.ViewTable(dal.GetCodingSessions());

        Console.Write($"\nWhich entry do you want to update? ");
        int id = UserInput.GetIdForUpdate();

        Viewer.DisplayPromptForTime("start");
        string startTime = UserInput.GetStartTime();

        Viewer.DisplayPromptForTime("finish");
        string endTime = UserInput.GetEndTime(Validator.ConvertToDate(startTime));

        TimeSpan duration = Validator.CalculateDuration(startTime, endTime);
        dal.UpdateEntry(id, startTime, endTime, duration.ToString());
    }
}