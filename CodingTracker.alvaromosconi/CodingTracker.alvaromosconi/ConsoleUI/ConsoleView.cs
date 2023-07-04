using ConsoleTableExt;

namespace CodingTracker.alvaromosconi.ConsoleUI;

internal class ConsoleView
{
    private const string OPTIONS = "0123456";
    internal void WelcomeMessage()
    {
        Console.WriteLine("======================================");
        Console.WriteLine("Coding Tracker Application");
        Console.WriteLine("Developed by: Alvaro Mosconi");
        Console.WriteLine("======================================");
    }
    internal void DisplayMainMenuOptions()
    {
        string options = $@"What would you like to do?

        1. Start a new Code Session.
        2. View all sessions.
        3. View all sessions between a certain range.
        4. Delete a session.";

        Console.WriteLine(options);
        Console.WriteLine();
    }

    internal void Menu()
    {
        var keepRunning = true;

        while (keepRunning)
        {
            DisplayMainMenuOptions();

            var selection = UserInput.SelectMenuOption(OPTIONS);
            keepRunning = selection switch
            {
                "1" => StartNewSession(),
                "2" => ViewAllSessions(),
                "3" => ViewSelectedSessions(),
                "4" => DeleteSelectedSession(),
                "0" => false,
                _ => true
            };
        }
    }
    private bool DeleteSelectedSession()
    {
        throw new NotImplementedException();
    }

    private bool ViewSelectedSessions()
    {
        throw new NotImplementedException();
    }

    private bool ViewAllSessions()
    {
        throw new NotImplementedException();
    }

    private bool StartNewSession()
    {
        throw new NotImplementedException();
    }
}




