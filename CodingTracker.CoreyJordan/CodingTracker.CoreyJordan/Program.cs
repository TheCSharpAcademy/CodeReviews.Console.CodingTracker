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
        //case "E":
        //    CloseCodingSession();
        //    return false;
        //case "D":
        //    DeleteCodingSession();
        //    return false;
        default:
            display.InvalidInput(userChoice);
            return false;
    }
}

void GetOpenCodingSessions()
{
    try
    {
        List<CodingSessionModel> sessions = CrudController.GetOpenSessions();
        display.DisplaySessions(sessions, State.Open.ToString());
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
}

void DeleteCodingSession()
{
    throw new NotImplementedException();
}

void CloseCodingSession()
{
    throw new NotImplementedException();
}

void CreateCodingSession()
{
    try
    {
        CrudController.CreateSession(UserInput.GetDate(Session.Start));
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

void GetCodingSessions()
{
    try
    {
        List<CodingSessionModel> sessions = CrudController.GetAllSessions();
        display.DisplaySessions(sessions, "Sessions");
        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
}