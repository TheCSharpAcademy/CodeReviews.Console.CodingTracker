namespace CodingTracker;

using System.Globalization;

class CodingSessionController
{
    private Database database;
    private MainMenuController? mainMenuController;

    public CodingSessionController(Database database)
    {
        this.database = database;
    }

    public void SetMainMenuController(MainMenuController controller)
    {
        mainMenuController = controller;
    }

    public void ShowCreateScreen()
    {
        var view = new CodingSessionCreateView(this);
        view.Show();
    }

    public void Create(string? start, string? end)
    {
        DateTime.TryParseExact(start, Configuration.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedStart);
        DateTime.TryParseExact(end, Configuration.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedEnd);
        var session = new CodingSession(parsedStart, parsedEnd);
        if (database.CreateCodingSession(session))
        {
            // TODO: Succes-Message
            BackToMainMenu();
        }
        else
        {
            // TODO: Error-Message
            BackToMainMenu();
        }
    }

    public void ShowList()
    {
        List<CodingSession> sessions = database.ReadAllCodingSessions();
        var view = new CodingSessionListView(this, sessions);
        view.Show();
    }

    public void ShowEditDelete(long codingSessionId)
    {
        var session = database.ReadCodingSession(codingSessionId);
        if (session == null)
        {
            ShowList();
        }
        else
        {
            var view = new CodingSessionView(this, session);
            view.Show();
        }
    }

    public void Edit(CodingSession session)
    {
        var view = new CodingSessionEditView(this, session);
        view.Show();
    }

    public void Update(CodingSession session)
    {
        database.UpdateCodingSession(session);
        ShowEditDelete(session.Id);
    }

    public void Delete(CodingSession session)
    {
        database.DeleteCodingSession(session.Id);
        ShowList();
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