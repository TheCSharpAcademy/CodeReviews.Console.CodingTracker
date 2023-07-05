using CodingTracker.alvaromosconi.Model;
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
        Console.WriteLine("==================================================");
        Console.WriteLine("             Coding Tracker Application           ");
        Console.WriteLine("             Developed by: Alvaro Mosconi         ");
        Console.WriteLine("==================================================");
        Console.WriteLine();
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
                '1' => StartNewSession(),
                '2' => ViewAllSessions(),
                '3' => ViewSelectedSessions(),
                '4' => DeleteSelectedSession(),
                '0' => false,
                _ => true
            };

            Console.Clear();
        }
    }
    private bool DeleteSelectedSession()
    {
        ViewSelectedSessions();
        Console.WriteLine("Type the id of the session that you want to be deteled.");
        string input = UserInput.GetIdFromUser();
    }

    private bool ViewSelectedSessions()
    {
        (DateTime from, DateTime till) = UserInput.GetDateRangeFromUser();
        List<CodeSessionModel> sessions = controller.GetSessionsInRange(from, till);
        Console.Clear();
        if (sessions.Count > 0)
        {
            ConsoleTableBuilder
            .From(sessions)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .WithTitle($"Sessions in range: {from.ToString()} to {till.ToString()}")
            .ExportAndWriteLine();
        }
        else
        {
            Console.WriteLine("\nNo sessions found in selected range.");
        }
        
        Console.ReadKey();
        
        return true;
    }

    private bool ViewAllSessions()
    {
        List<CodeSessionModel> sessions = controller.GetAllSessions();

        Console.Clear();
        if (sessions.Count > 0)
        {
            ConsoleTableBuilder
                .From(sessions)
                .WithFormat(ConsoleTableBuilderFormat.Default)
                .WithTitle("Sessions", ConsoleColor.Black, ConsoleColor.White)
                .ExportAndWriteLine();
        }
        else
            Console.WriteLine("\nNo sessions found.");

        Console.ReadKey();
        return true;
    }

    private bool StartNewSession()
    {
        Console.Clear();
        Console.WriteLine("Press S whenever you are ready!");
        char input = Console.ReadKey().KeyChar;
        Console.Clear();

        if (input == 's' || input == 'S')
        { 
            DateTime startTime = DateTime.Now;
            Console.WriteLine("_______________________________________________");
            Console.WriteLine("Session has started! May the Force be with you.");
            Console.WriteLine("          Press ANY KEY to finish.             ");
            Console.WriteLine( "______________________________________________");
            Console.ReadKey();
            EndSession(startTime);
        }
        Console.Clear();
        return true;
    }

    private void EndSession(DateTime start)
    {
        DateTime end = DateTime.Now;
        controller.SaveSession(start, end);
    }
}




