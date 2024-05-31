using System.Configuration;

public class App
{
    private DatabaseManager _databaseManager = new DatabaseManager(ConfigurationManager.ConnectionStrings["CodingTrackerDB"].ConnectionString);

    private UserInput _userInput = new UserInput();
    private LiveTracker _liveTracker;

    public void Run()
    {
        _databaseManager.InitializeDatabase();

        while (true)
        {
            var mainMenuOptions = _userInput.MainMenu();

            switch (mainMenuOptions)
            {
                case MainMenuOptions.NewSession:
                    NewSession();
                    break;
                case MainMenuOptions.ExistingSession: break;
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
            TimeSpan ts = new TimeSpan(_liveTracker.Stopwatch.Elapsed.Hours, _liveTracker.Stopwatch.Elapsed.Minutes, _liveTracker.Stopwatch.Elapsed.Seconds);
            var newSessionOptions = _userInput.NewSessionMenu(ts);

            switch (newSessionOptions)
            {
                case NewSessionOptions.Start:
                    _liveTracker.Start();
                    break;
                case NewSessionOptions.Stop:
                    _liveTracker.Stop();
                    break;
                case NewSessionOptions.Reset:
                    _liveTracker.Reset();
                    break;
                case NewSessionOptions.Update:
                    break;
                case NewSessionOptions.Exit:
                    _liveTracker.Stop();
                    exit = true;
                    return;
            } 
        }
    }
}