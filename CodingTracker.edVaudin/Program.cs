namespace CodingTracker;

class Program
{
    static bool endApp = false;
    static void Main(string[] args)
    {
        Viewer.DisplayTitle();
        UserController.InitializeDatabase();

        while (!endApp)
        {
            ContinueUserInteraction();
        }

        ExitApp();
    }

    private static void ContinueUserInteraction()
    {
        Viewer.DisplayOptionsMenu();
        string userInput = UserInput.GetUserOption();
        UserController.ProcessInput(userInput);
    }

    public static void SetEndAppToTrue() => endApp = true;

    private static void ExitApp() => Environment.Exit(0);
}

