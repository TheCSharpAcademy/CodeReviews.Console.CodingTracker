namespace CodingTracker;

using System.Globalization;

class CodingSessionController
{
    private readonly Database database;
    private MainMenuController? mainMenuController;
    private SortOrder sortOrder = SortOrder.Ascending;
    private FilterPeriod filterPeriod = FilterPeriod.None;

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
        ShowCreateScreen(null);
    }

    public void ShowCreateScreen(string? message)
    {
        var view = new CodingSessionCreateView(this);
        view.SetMessage(message);
        view.Show();
    }

    public void Create(string? start, string? end)
    {
        if (!DateTime.TryParseExact(start, Configuration.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedStart))
        {
            ShowCreateScreen("ERROR - Invalid format entered for Start.");
        }

        if (!DateTime.TryParseExact(end, Configuration.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedEnd))
        {
            ShowCreateScreen("ERROR - Invalid format entered for End.");
        }
        else if (end.CompareTo(start) <= 0)
        {
            ShowCreateScreen("ERROR - End must be after Start.");
        }

        var session = new CodingSession(parsedStart, parsedEnd);
        if (database.CreateCodingSession(session))
        {
            BackToMainMenu("OK - Session successfully saved.");
        }
        else
        {
            BackToMainMenu("ERROR - Failed to save new session.");
        }
    }

    public void ShowList()
    {
        ShowList(null);
    }

    public void ShowList(string? message)
    {
        List<CodingSession> sessions = database.ReadAllCodingSessions(sortOrder, filterPeriod);
        var view = new CodingSessionListView(this, sessions);
        view.SetMessage(message);
        view.Show();
    }

    public void SetSortOrder(SortOrder sortOrder)
    {
        this.sortOrder = sortOrder;
    }

    public void SetFilterPeriod(FilterPeriod filterPeriod)
    {
        this.filterPeriod = filterPeriod;
    }

    public void ShowEditDelete(long codingSessionId)
    {
        ShowEditDelete(codingSessionId, null);
    }

    public void ShowEditDelete(long codingSessionId, string? message)
    {
        var session = database.ReadCodingSession(codingSessionId);
        if (session == null)
        {
            ShowList($"ERROR - Session #{codingSessionId} not found.");
        }
        else
        {
            var view = new CodingSessionView(this, session);
            view.SetMessage(message);
            view.Show();
        }
    }

    public void Edit(CodingSession session)
    {
        var view = new CodingSessionEditView(this, session);
        view.Show();
    }

    public void Update(long sessionId, string? start, string? end)
    {
        if (!DateTime.TryParseExact(start, Configuration.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedStart))
        {
            ShowCreateScreen("ERROR - Invalid format entered for Start.");
        }

        if (!DateTime.TryParseExact(end, Configuration.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedEnd))
        {
            ShowCreateScreen("ERROR - Invalid format entered for End.");
        }
        else if (end.CompareTo(start) <= 0)
        {
            ShowCreateScreen("ERROR - End must be after Start.");
        }

        var session = new CodingSession(sessionId, parsedStart, parsedEnd);
        if (database.UpdateCodingSession(session))
        {
            ShowEditDelete(session.Id, "OK - Session successfully saved.");
        }
        else
        {
            ShowEditDelete(session.Id, "ERROR - Failed to save new session.");
        }
    }

    public void Delete(CodingSession session)
    {
        if (database.DeleteCodingSession(session.Id))
        {
            ShowList($"OK - Session #{session.Id} deleted.");
        }
        else
        {
            ShowList($"ERROR- Failed to delete session #{session.Id}.");
        }
    }

    public void BackToMainMenu()
    {
        BackToMainMenu(null);
    }

    public void BackToMainMenu(string? message)
    {
        if (mainMenuController == null)
        {
            throw new InvalidOperationException("Required MainMenuController missing.");
        }
        mainMenuController.ShowMainMenu(message);
    }
}