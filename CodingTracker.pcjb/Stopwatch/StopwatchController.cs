namespace CodingTracker;

class StopwatchController
{
    private readonly Database database;
    private readonly Stopwatch stopwatch;
    private MainMenuController? mainMenuController;

    public StopwatchController(Database database)
    {
        this.database = database;
        stopwatch = new Stopwatch();
    }

    public void SetMainMenuController(MainMenuController controller)
    {
        mainMenuController = controller;
    }

    public void Show()
    {
        var view = new StopwatchView(this, stopwatch);
        view.Show();
    }

    public void Start()
    {
        stopwatch.Start();
        var view = new StopwatchView(this, stopwatch);
        view.Show();
    }

    public void Stop()
    {
        var session = stopwatch.Stop();
        var view = new StopwatchView(this, stopwatch);
        if (session == null)
        {
            view.SetMessage("ERROR - Failed get session from stopwatch.");
        }
        else if (database.CreateCodingSession(session))
        {
            view.SetMessage("OK - Session successfully saved.");
        }
        else
        {
            view.SetMessage("ERROR - Failed to save new session.");
        }
        view.Show();
    }

    public void BackToMainMenu()
    {
        if (mainMenuController == null)
        {
            throw new InvalidOperationException("Required MainMenuController missing.");
        }
        mainMenuController.ShowMainMenu();
    }
}