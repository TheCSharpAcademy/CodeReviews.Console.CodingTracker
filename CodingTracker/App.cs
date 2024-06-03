using System.Configuration;

public class App
{
    private DatabaseManager _databaseManager = new DatabaseManager(ConfigurationManager.ConnectionStrings["CodingTrackerDB"].ConnectionString);
    private UserInput _userInput = new UserInput();
    private LiveTracker _liveTracker;
    private CodingRepository _codingRepository;

    public void Run()
    {
        _databaseManager.InitializeDatabase();

        while (true)
        {
            var mainMenuOptions = _userInput.MainMenu();
            _codingRepository = new CodingRepository(_databaseManager);

            switch (mainMenuOptions)
            {
                case MainMenuOptions.NewSession:
                    NewSession();
                    break;
                case MainMenuOptions.AddManualSession: break;
                case MainMenuOptions.ViewSessions: break; // Let the user filter their coding records per period (weeks, days, years) and/or order ascending or descending.
                case MainMenuOptions.Goals: break;
                case MainMenuOptions.Reports: break; // options (years, weeks, days). Breakdown by filter. 
                case MainMenuOptions.Exit: break;
            }
        }
    }

    public void NewSession()
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
                    _codingRepository.AddSession(_liveTracker.Save());
                    exit = true;
                    return;
            }
        }
    }
}