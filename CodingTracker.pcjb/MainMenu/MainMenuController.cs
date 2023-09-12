namespace CodingTracker;

class MainMenuController
{
    private CodingSessionController? codingSessionController;

    public void SetCodingSessionController(CodingSessionController controller)
    {
        codingSessionController = controller;
    }

    public void ShowMainMenu()
    {
        ShowMainMenu(null);
    }

    public void ShowMainMenu(string? message)
    {
        var view = new MainMenuView(this);
        view.SetMessage(message);
        view.Show();
    }

    public void NewCodingSession()
    {
        if (codingSessionController == null)
        {
            throw new InvalidOperationException("Required CodingSessionController missing.");
        } 
        codingSessionController.ShowCreateScreen();
    }


    public void ListCodingSessions()
    {
        if (codingSessionController == null)
        {
            throw new InvalidOperationException("Required CodingSessionController missing.");
        } 
        codingSessionController.ShowList();
    }

    public void Exit()
    {
        var view = new ExitView();
        view.Show();
    }
}