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
                    break;
                case MainMenuOptions.Goals: 
                    /*
                     * Goals Table Structure:
                     * Fields: Id | Name | Total Hours | Start Date | End Date | Completion Status
                     *
                     * Menu Options:
                     * 1. Set New Goal:
                     *    - Prompt the user to input the total amount of coding hours for the new goal.
                     *
                     * 2. View Current Goals:
                     *    - Display a list of all goals with details such as name, total hours, start date, and completion status. Color completed ones green.
                     *    - Allow the user to select a goal for more information.
                     *    - Output additional details including remaining hours, days remaining, and a table of related coding sessions.
                     *
                     * 3. Delete a Goal:
                     *    - Output all goals in a table format and prompt the user to select a goal ID for deletion.
                     *
                     * 4. Insert Test Goal Data:
                     *    - Generate and insert test data for goals into the goals table.
                     *    
                     * Information on implementation:
                     * When displaying information about a selected goal, filter the coding sessions:
                     * - Retrieve coding sessions with a start date after the start date of the selected goal.
                     * - Exclude coding sessions with a start date after the completion date of the selected goal.
                     * This ensures only relevant coding sessions are included in the output, limited to the duration of the goal.
                     */

                    break;
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
            var sessionList = _codingRepository.GetSessions();
            var sortOrber = _userInput.GetSortingOrder();
            var filter = _userInput.FilteringOptionsMenu();

            switch (filter)
            {
                case FilteringOptions.Dates:
                    var option = _userInput.GetUserFilterPeriod();
                    int length = _userInput.FilterDuration(option);
                    var filterSession = sessionList.Where(e => e.StartTime >= DateTime.Today.AddDays(-length));
                    _userInput.OutputSessions(filterSession, sortOrber);
                    break;
                case FilteringOptions.All:
                    _userInput.OutputSessions(sessionList, sortOrber);
                    break;
            }
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