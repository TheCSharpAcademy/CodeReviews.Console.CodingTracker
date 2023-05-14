using CodingTracker.CoreyJordan;
using CodingTrackerLibrary;
using System.Diagnostics;

CrudController.InitDatabase();
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
        default:
            display.InvalidInput(userChoice);
            break;
    }
    return exit;
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
    Console.WriteLine("Press any key to log session.");
    Console.ReadKey();

    try
    {
        CrudController.CreateLiveSession(start, end);
        display.Success("\nSession logged");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        Console.Write("Press any key...");
        Console.ReadKey();
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
        Console.Write("Press any key...");
        Console.ReadKey();
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

        Console.Write($"Are you sure you wish to delete coding session ");
        Console.Write($"{allSessions.First(x => x.SessionId == key).SessionId} Y/N: ");
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
        Console.Write("Press any key...");
        Console.ReadKey();
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
        Console.Write("Press any key...");
        Console.ReadKey();
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
        Console.Write("Press any key...");
        Console.ReadKey();
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
        Console.Write("Press any key...");
        Console.ReadKey();
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
            CodingReportModel report = new(sessions);
            display.DisplayReport(report);
            break;
        case "P":
            FilterByDate();
            break;
        case "R":
            FilterByRange();
            break;
        default:
            display.InvalidInput(userChoice);
            break;
    }
    return exit;
}

void FilterByRange()
{
    throw new NotImplementedException();
}

void FilterByDate()
{
    throw new NotImplementedException();
}