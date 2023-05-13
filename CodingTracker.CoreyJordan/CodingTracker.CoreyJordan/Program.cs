using CodingTracker.CoreyJordan;
using CodingTrackerLibrary;

CrudController.InitDatabase();
ConsoleDisplay display = new();

bool exitApp = false;
while(!exitApp)
{
    display.DisplayOpenSessions();
    display.DisplayMainMenu();
    exitApp = ExecuteUserChoice(UserInput.GetString());
}

bool ExecuteUserChoice(string userChoice)
{
    switch (userChoice.ToUpper())
    {
        case "Q":
            return true;
        //case "V":
        //    GetCodingSessions();
        //    return false;
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
    throw new NotImplementedException();
}