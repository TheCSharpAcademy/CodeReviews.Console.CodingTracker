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
                case MainMenuOptions.ViewSessions: break; // Let the user filter their coding records per period (weeks, days, years) and/or order ascending or descending.
                case MainMenuOptions.Goals: break;
                case MainMenuOptions.Reports: break; // options (years, weeks, days). Breakdown by filter. 
                case MainMenuOptions.Exit: break;
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
        _codingRepository.AddSession(codingSession);
    }
}