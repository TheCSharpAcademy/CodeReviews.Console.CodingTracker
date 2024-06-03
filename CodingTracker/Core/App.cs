using Spectre.Console;
using System.Configuration;

public class App
{
    private DatabaseManager _databaseManager = new DatabaseManager(ConfigurationManager.ConnectionStrings["CodingTrackerDB"].ConnectionString);
    private UserInput _userInput;
    private LiveTracker _liveTracker;
    private CodingRepository _codingRepository;
    private ValidateInput _validateInput;

    public void Run()
    {
        _databaseManager.InitializeDatabase();
        _validateInput = new ValidateInput();
        _userInput = new UserInput(_validateInput);
        _codingRepository = new CodingRepository(_databaseManager);

        while (true)
        {
            var mainMenuOptions = _userInput.MainMenu();

            switch (mainMenuOptions)
            {
                case MainMenuOptions.NewSession:
                    NewSession();
                    break;
                case MainMenuOptions.AddManualSession: 
                    AddManualSession();
                    break;
                case MainMenuOptions.ViewSessions: 
                    ViewSessions();
                    break; // Let the user filter their coding records per period (weeks, days, years) and/or order ascending or descending.
                case MainMenuOptions.Goals: break;
                case MainMenuOptions.Reports: break; // options (years, weeks, days). Breakdown by filter. 
                case MainMenuOptions.InsertTestData:
                    InsertTestData();
                    break; 
                case MainMenuOptions.Exit: 
                    Environment.Exit(0);
                    break;
            }
        }
    }

    private void NewSession()
    {
        _liveTracker = new LiveTracker();
        bool exit = false;

        while (!exit)
        {
            var newSessionOptions = _userInput.NewSessionMenu(_liveTracker.GetTime(), _liveTracker.Stopwatch.IsRunning);

            switch (newSessionOptions)
            {
                case NewSessionOptions.Start:
                    _liveTracker.Start();
                    break;
                case NewSessionOptions.Reset:
                    _liveTracker.Reset();
                    break;
                case NewSessionOptions.Update:
                    break;
                case NewSessionOptions.Exit:
                    CodingSession codingSession = _liveTracker.Save();
                    _codingRepository.AddSession(codingSession);
                    exit = true;
                    return;
            }
        }
    }

    private void AddManualSession()
    {
        CodingSession codingSession = _userInput.AddManualSession();

        if (codingSession.EndTime > codingSession.StartTime)
        {
            _codingRepository.AddSession(codingSession);
        }
    }

    private void ViewSessions()
    {
        var getSessions = _codingRepository.GetSessions();

        if (_codingRepository.GetSessions().Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No entries in database to filter. Enter any key to continue.[/]");
            Console.ReadKey(true);
        }
        else
        {
            /* Build a menu to prompt the user for their filtering preferences.
             * Ensure that the list of sessions to filter is not empty.
             * Offer options for filtering in ascending or descending order.
             * Provide an additional submenu with the following options:
             *   - Filter by date (weeks, days, years)
             *     - If this option is selected, prompt the user to choose from one of the three available options.
             *       Once selected, ask the user to input an integer. For example, if they choose "weeks," entering 3 would represent the past 3 weeks of sessions.
             *   - Show all sessions
             */
        }

        
    }

    private void InsertTestData()
    {
        var number = _userInput.InsertTestData();
        _codingRepository.InsertTestData(number);
        AnsiConsole.MarkupLine($"[green]{number} random records have been inserted[/]");
        Console.ReadKey(true);
    }
}