using CodingTracker.CoreyJordan;
using CodingTrackerLibrary;
using System.Diagnostics;

CrudController.InitDatabase();
CrudController.InitGoalTable();
ConsoleDisplay display = new();

bool exitApp = false;
while(!exitApp)
{
    GetOpenCodingSessions();
    display.DisplayMainMenu();
    exitApp = ExecuteUserChoice(UserInput.GetString());
}


bool ExecuteUserChoice(string userChoice)
{
    bool exit = false;
    switch (userChoice.ToUpper())
    {
        case "Q":
            exit = true;
            break;
        case "V":
            GetCodingSessions();
            break;
        case "N":
            CreateCodingSession();
            break;
        case "E":
            CloseCodingSession();
            break;
        case "L":
            RecordLiveSession();
            break;
        case "D":
            DeleteCodingSession();
            break;
        case "S":
            SetGoal();
            break;
        default:
            display.InvalidInput(userChoice);
            break;
    }
    return exit;
}

void SetGoal()
{
    Console.Write("Enter a name for this goal: ");
    string name = UserInput.GetString();

    int goal = UserInput.GetInteger("Enter how many days you wish to spend on this goal: ");

    try
    {
        CrudController.CreateGoal(name, goal);
        display.Success("Goal created");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        display.ContinuePrompt();
    }
}

void RecordLiveSession()
{
    Stopwatch liveSession = new();

    Console.Clear();
    Console.WriteLine("Press any key to begin...");
    Console.ReadKey();
    DateTime start = DateTime.Now;
    liveSession.Start();

    Console.WriteLine("\nRecording - press any key to end session...");
    Console.ReadKey();
    DateTime end = DateTime.Now;
    liveSession.Stop();

    Console.WriteLine($"\nYour coding session lasted {liveSession.Elapsed}");
    display.ContinuePrompt();

    try
    {
        CrudController.CreateLiveSession(start, end);
        display.Success("\nSession logged");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        display.ContinuePrompt();
    }
}

void GetOpenCodingSessions()
{
    try
    {
        List<CodingSessionModel> openSessions = CrudController.GetOpenSessions();
        display.DisplaySessions(openSessions, State.Open.ToString());
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        display.ContinuePrompt();
    }
}

void DeleteCodingSession()
{
    try
    {
        List<CodingSessionModel> allSessions = CrudController.GetAllSessions();
        display.DisplaySessions(allSessions, "All Coding Sessions");

        int key = UserInput.GetInteger("Select a session: ");
        while (!DataValidation.IsInRange(key, allSessions))
        {
            display.InvalidInput(key.ToString());
            display.DisplaySessions(allSessions, "All Coding Sessions");
            key = UserInput.GetInteger("Select a session: ");
        }

        Console.Write($"Are you sure you wish to delete coding session {key} Y/N: ");
        string yesNo = Console.ReadLine()!;
        if (yesNo.ToUpper() == "Y")
        {
            CrudController.DeleteSession(key);
            display.Success("Session deleted");
        }
        else
        {
            display.Success("Canceled");
        }

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        display.ContinuePrompt();
    }
}

void CloseCodingSession()
{
    try
    {
        List<CodingSessionModel> openSessions = CrudController.GetOpenSessions();
        display.DisplaySessions(openSessions, $"{State.Open} Sessions");

        int key = UserInput.GetInteger("Select a session: ");
        while (!DataValidation.IsInRange(key, openSessions))
        {
            display.InvalidInput(key.ToString());
            display.DisplaySessions(openSessions, $"{State.Open} Sessions");
            key = UserInput.GetInteger("Select a session: ");
        }

        DateTime startDate = openSessions.FirstOrDefault(x => x.SessionId == key)!.StartTime;
        DateTime endDate = UserInput.GetDate(Session.Finish);
        while (!DataValidation.IsChronological(startDate, endDate))
        {
            display.InvalidInput($"{endDate} is prior to start date and");
            endDate = UserInput.GetDate(Session.Finish);
        }

        CrudController.CloseSession(UserInput.GetDate(Session.Finish), key);
        display.Success("Session updated");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        display.ContinuePrompt();
    }
}

void CreateCodingSession()
{
    DateTime startDate = UserInput.GetDate(Session.Start);
    while (!DataValidation.IsNotFuture(startDate))
    {
        display.InvalidInput($"{startDate:g} is in the future and");
        startDate = UserInput.GetDate(Session.Start);
    }

    try
    {
        CrudController.CreateSession(startDate);
        display.Success("Session created");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        display.ContinuePrompt();
    }
}

void GetCodingSessions()
{
    try
    {
        List<CodingSessionModel> allSessions = CrudController.GetAllSessions();
        bool returnToMain = false;
        while (!returnToMain)
        {
            display.DisplaySessions(allSessions, "All Coding Sessions");
            display.SessionMenu();
            returnToMain = ExecuteSessionChoice(UserInput.GetString(), allSessions);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        display.ContinuePrompt();
    }
}

bool ExecuteSessionChoice(string userChoice, List<CodingSessionModel> sessions)
{
    bool exit = false;
    switch (userChoice.ToUpper())
    {
        case "X":
            exit = true;
            break;
        case "G":
            GenerateReport(sessions);
            break;
        case "F":
            FilterByDate(sessions);
            break;
        case "R":
            FilterByRange(sessions);
            break;
        case "D":
            DeleteCodingSession();
            break;
        default:
            display.InvalidInput(userChoice);
            break;
    }
    return exit;
}

void GenerateReport(List<CodingSessionModel> sessions)
{
    CodingReportModel report = new(sessions);
    CodingGoalModel goal = new();
    try
    {
        goal = CrudController.GetGoal();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        display.ContinuePrompt();
    }

    display.DisplayReport(report, goal);
}

void FilterByRange(List<CodingSessionModel> sessions)
{
    display.RangeMenu();
    string userChoice = UserInput.GetString();
    DateTime end = DateTime.Now;
    DateTime start;
    
    switch (userChoice.ToUpper())
    {
        case "D":
            int days = UserInput.GetInteger($"Enter number of {Span.Days}: ");
            start = end - TimeSpan.FromDays(days);
            display.DisplaySessions(FilterSessions(sessions, start, end), $"Last {days} {Span.Days}");
            display.ContinuePrompt();
            break;
        case "W":
            int weeks = UserInput.GetInteger($"Enter number of {Span.Weeks}: ");
            start = end - TimeSpan.FromDays(weeks * 7);
            display.DisplaySessions(FilterSessions(sessions, start, end), $"Last {weeks} {Span.Weeks}");
            display.ContinuePrompt();
            break;
        case "M":
            int months = UserInput.GetInteger($"Enter number of {Span.Months}: ");
            start = end - TimeSpan.FromDays(months * 30);
            display.DisplaySessions(FilterSessions(sessions, start, end), $"Last {months} {Span.Months}");
            display.ContinuePrompt();
            break;
        case "Y":
            int years = UserInput.GetInteger($"Enter number of {Span.Years}: ");
            start = end - TimeSpan.FromDays(years * 365);
            display.DisplaySessions(FilterSessions(sessions, start, end), $"Last {years} {Span.Years}");
            display.ContinuePrompt();
            break;
        default:
            display.InvalidInput(userChoice);
            break;
    }
}

void FilterByDate(List<CodingSessionModel> sessions)
{
    DateTime start = UserInput.GetDate(Session.Start);
    DateTime end = UserInput.GetDate(Session.Finish);

    display.DisplaySessions(FilterSessions(sessions, start, end), $"Sessions started between {start} and {end}");
    display.ContinuePrompt();
}

static List<CodingSessionModel> FilterSessions(List<CodingSessionModel> sessions, DateTime start, DateTime end)
{
    List<CodingSessionModel> filteredSessions = new();
    foreach (CodingSessionModel session in sessions)
    {
        if (session.StartTime > start && session.StartTime < end)
        {
            filteredSessions.Add(session);
        }
    }

    return filteredSessions;
}