using CodingTracker.CoreyJordan;
using CodingTrackerLibrary;

CrudController.InitDatabase();
ConsoleDisplay display = new();
UserInput input = new();

bool exitApp = false;
while(!exitApp)
{
    display.DisplayOpenSessions();
    display.DisplayMainMenu();
    exitApp = ExecuteUserChoice(input.GetString());
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
        //case "N":
        //    CreateCodingSession();
        //    return false;
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
    throw new NotImplementedException();
}

void GetCodingSessions()
{
    throw new NotImplementedException();
}