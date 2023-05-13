using CodingTracker.CoreyJordan;
using CodingTrackerLibrary;
using System.Security.Cryptography.X509Certificates;

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
    try
    {
        List<CodingSessionModel> sessions = CrudController.GetOpenSessions();
        display.DisplaySessions(sessions, "Sessions");

        int key = UserInput.GetInteger("Select a session: ");
        while (!sessions.Any(x => x.SessionId == key))
        {
            display.InvalidInput(key.ToString());
            display.DisplaySessions(sessions, "Sessions");
            key = UserInput.GetInteger("Select a session: ");
        }

        CrudController.CloseSession(UserInput.GetDate(Session.Finish), key);
        display.Success("Session updated");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
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
        Console.WriteLine(ex.Message);
    }
}