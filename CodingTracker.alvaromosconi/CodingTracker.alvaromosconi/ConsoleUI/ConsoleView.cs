using CodingTracker.alvaromosconi.Controller;
using CodingTracker.alvaromosconi.Model;
using ConsoleTableExt;

namespace CodingTracker.alvaromosconi.ConsoleUI;

internal class ConsoleView
{

    private const string OPTIONS = "0123456";
    private const string BACK = "0";
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
        Console.WriteLine("==================================================\n");
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
            Console.WriteLine("______________________________________________");
            Console.ReadKey();
            EndSession(startTime);
        }
        Console.Clear();

        return true;
    }

    private bool ViewAllSessions()
    {
        Console.Clear();
        List<CodeSessionModel> sessions = controller.GetAllSessions();
        DisplaySessions(sessions);
        Console.ReadKey();

        return true;
    }

    private bool ViewSelectedSessions()
    {
        Console.Clear();
        Console.WriteLine("Search by date\n ");

        string from = UserInput.GetDateRangeFromUser("FROM");
        string till = UserInput.GetDateRangeFromUser("TILL");
        if (from != BACK && till != BACK)
        {
            List<CodeSessionModel> sessions = controller.GetSessionsInRange(DateTime.Parse(from), DateTime.Parse(till));
            DisplaySessions(sessions);
            Console.ReadKey();
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Invalid dates.");
            Console.ReadKey();
        }
        
        return true;
    }

    private bool DeleteSelectedSession()
    {
        List<CodeSessionModel> sessions = controller.GetAllSessions();
        DisplaySessions(sessions);

        if (sessions.Count > 0)
        {
            int id = UserInput.GetIdFromUser(sessions);

            if (id != 0)
            {
                controller.DeleteSession(id);
                Console.WriteLine("\nThe session has been deleted.");
            }
        }
        
        Console.ReadKey();
        
        return true;
    }

    private static void DisplaySessions(List<CodeSessionModel> sessions)
    {
        Console.Clear();
        if (sessions.Count > 0)
        {
            ConsoleTableBuilder
            .From(sessions)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .WithTitle($"Sessions")
            .ExportAndWriteLine();
        }
        else
        {
            Console.WriteLine("\nNo sessions found.");
        }
    }

    private void EndSession(DateTime start)
    {
        DateTime end = DateTime.Now;
        controller.SaveSession(start, end);
    }
}




