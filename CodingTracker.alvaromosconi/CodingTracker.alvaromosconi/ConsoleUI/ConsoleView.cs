using ConsoleTableExt;

namespace CodingTracker.alvaromosconi.ConsoleUI;

internal class ConsoleView
{

    private const string OPTIONS = "0123456";
    private CodingController controller;

    public ConsoleView(CodingController controller)
    {
        this.controller = controller;
    }

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
        Console.WriteLine("Press S whenever you are ready!");
        string input = Console.ReadLine().Trim().ToLower();

        if (input == "s")
        {
            DateTime startTime = DateTime.Now;
            Console.WriteLine("_______________________________________________");
            Console.WriteLine("Session has started! May the Force be with you.");
            Console.WriteLine("         Press ANY KEY to finish.              ");
            Console.WriteLine( "______________________________________________");
            Console.ReadKey();
            EndSession(startTime);
        }

        return true;
    }

    private void EndSession(DateTime start)
    {
        DateTime end = DateTime.Now;
        controller.SaveSession(start, end);
    }
}




