using CodingTracker.kraven88.Data;

namespace CodingTracker.kraven88.ConsoleUI;

internal class MainMenu
{
    public static string nl = Environment.NewLine;
    private DataAccess db;

    public MainMenu()
    {
        db = new DataAccess();
    }

    public void Header()
    {
        Console.WriteLine("==============================");
        Console.WriteLine("  Coding Tracker by kraven88  ");
        Console.WriteLine("==============================" + nl);
    }

    public void Menu()
    {
        Header();
        DisplayMenuItems();

        var isRunning = true;
        while (isRunning)
        {
            var selection = Helpers.SelectMenuItem("1234560");
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

    private void DisplayMenuItems()
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

    public bool StartNewSession()
    {
        // TODO
        throw new NotImplementedException();
    }

    public void EndCurrentSession()
    {
        // TODO
        throw new NotImplementedException();
    }
}
