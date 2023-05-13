using CodingTracker.CoreyJordan;
using CodingTrackerLibrary;

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
    switch (userChoice.ToUpper())
    {
        case "Q":
            return true;
        case "V":
            GetCodingSessions();
            return false;
        case "N":
            CreateCodingSession();
            return false;
        case "E":
            CloseCodingSession();
            return false;
        case "D":
            DeleteCodingSession();
            return false;
        default:
            display.InvalidInput(userChoice);
            return false;
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
    try
    {
        CrudController.CreateSession(UserInput.GetDate(Session.Start));
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
        display.DisplaySessions(allSessions, "All Coding Sessions");
        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        Console.Write("Press any key...");
        Console.ReadKey();
    }
}