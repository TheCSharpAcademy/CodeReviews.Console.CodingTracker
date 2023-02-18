using CodingTracker.kraven88.Data;
using CodingTracker.kraven88.Models;
using ConsoleTableExt;

namespace CodingTracker.kraven88.ConsoleUI;

internal class MainMenu
{
    public static string nl = Environment.NewLine;
    private DataAccess db;

    public MainMenu()
    {
        db = new DataAccess();
    }

    private bool DeleteSelectedSession()
    {
        Header();
        var session = new CodingSession();
        Console.WriteLine("Search for sessions to delete." + nl);
        
        session.Start = DateTime.Parse(UserInput.AskForDate("From"));
        session.End = DateTime.Parse(UserInput.AskForDate("Till")).AddDays(1); 
        
        var list = db.LoadSelectedSessions(session);

        ConsoleTableBuilder
            .From(list)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .WithTitle("Selected for deletion", ConsoleColor.Black, ConsoleColor.White)
            .ExportAndWriteLine();

        var id = int.Parse(UserInput.AskForID(list));
        if (id == 0) return true;

        session.Id = id;
        db.DeleteSession(session);
        Console.WriteLine("Session succesfuly deleted.");

        Console.ReadLine();
        return true;
    }
    
    private static void DisplayMenuItems()
    {
        var menu = $@"What would you like to do?
    1 - Start new Coding Session
    2 - Log previous session
    3 - View last session
    4 - View all sessions
    5 - View sessions for selected dates
    6 - Delete selected session
    0 - Exit
    ";
        Console.WriteLine(menu);
    }
    
    public void EndCurrentSession(DateTime start)
    {
        var s = new CodingSession();
        s.Start = start;
        var end = DateTime.Now;
        s.End = end.AddTicks(-(end.Ticks % TimeSpan.TicksPerSecond));

        db.SaveSession(s);
        Console.WriteLine("Session saved!");
        Console.ReadLine();
    }
    
    public static void Header()
    {
        Console.Clear();
        Console.WriteLine("==============================");
        Console.WriteLine("  Coding Tracker by kraven88  ");
        Console.WriteLine("==============================" + nl);
    }
    
    private bool LogPreviousSession()
    {
        Header();

        var session = new CodingSession();
        var isValidSession = false;

        while (isValidSession == false)
        {
            var startDate = UserInput.AskForDate("start");
            var startTime = UserInput.AskForTime();
            session.Start = DateTime.Parse($"{startDate}T{startTime}");

            var endDate = UserInput.AskForDate("end");
            var endTime = UserInput.AskForTime();
            session.End = DateTime.Parse($"{endDate}T{endTime}");
            isValidSession = ValidateSession(session.Start, session.End);
            if (isValidSession == false)
            {
                Console.WriteLine("Invalid dates. A session cannot end before it begun." + nl);
            }

        }

        db.SaveSession(session);
        Console.WriteLine("Session logged succesfuly");
        Console.ReadKey();

        return true;
    }
    
    public void Menu()
    {
        var isRunning = true;

        while (isRunning)
        {
            Header();
            DisplayMenuItems();

            var selection = UserInput.SelectMenuItem("1234560");
            isRunning = selection switch
            {
                "1" => StartNewSession(),
                "2" => LogPreviousSession(),
                "3" => ViewLastSession(),
                "4" => ViewAllSessions(),
                "5" => ViewSelectedSessions(),
                "6" => DeleteSelectedSession(),
                "0" => false,
                _ => true
            };
        }
    }
    
    public bool StartNewSession()
    {
        Header();

        Console.Write("Would you like to start a new session (Y/N): ");
        var input = Console.ReadLine()!.Trim().ToUpper();
        if (input == "Y")
        {
            var start = DateTime.Now;
            start = start.AddTicks(-(start.Ticks % TimeSpan.TicksPerSecond));
            Console.WriteLine("______________________________" + nl);
            Console.WriteLine("     Session in progress!     ");
            Console.WriteLine("    Press ENTER to finish.    ");
            Console.WriteLine("______________________________");
            Console.ReadLine();
            EndCurrentSession(start);
        }

        return true;
    }
    
    private static bool ValidateSession(DateTime start, DateTime end)
    {
        var output = false;
        if (end > start)
            output = true;

        return output;
    }
    
    private bool ViewAllSessions()
    {
        Header();

        var list = db.LoadAllSessions();

        if (list.Count > 0)
        {
            ConsoleTableBuilder
                .From(list)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithTitle("All Sessions", ConsoleColor.Black, ConsoleColor.White)
                .ExportAndWriteLine();
            Console.WriteLine($"Average duration:\t{list.AverageDuration()}");
            Console.WriteLine($"Total duration:\t{list.TotalDuration()}");
        }
        else
            Console.WriteLine(nl + "No sessions found.");

        Console.ReadKey();

        return true;
    }
    
    private bool ViewLastSession()
    {
        Header();

        var list = db.LoadLastSession();

        if (list.Count > 0)
        {
            ConsoleTableBuilder
                .From(list)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithTitle("Last Session", ConsoleColor.Black, ConsoleColor.White)
                .ExportAndWriteLine();
        }
        else
            Console.WriteLine(nl + "No sessions found.");

        Console.ReadKey();

        return true;
    }
    
    private bool ViewSelectedSessions()
    {
        Header();
        var session = new CodingSession();
        session.Start = DateTime.Parse(UserInput.AskForDate("From"));
        session.End = DateTime.Parse(UserInput.AskForDate("Till")).AddDays(1);

        var list = db.LoadSelectedSessions(session);

        if (list.Count > 0)
        {
            ConsoleTableBuilder
                .From(list)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithTitle("Selected Sessions", ConsoleColor.Black, ConsoleColor.White)
                .ExportAndWriteLine();
            if (list.Count > 1)
            {
                Console.WriteLine($"Average duration:\t{list.AverageDuration()}");
                Console.WriteLine($"Total duration:\t{list.TotalDuration()}");
            }
        }
        else
            Console.WriteLine(nl + "No sessions found.");

        Console.ReadKey();

        return true;
    }
}
