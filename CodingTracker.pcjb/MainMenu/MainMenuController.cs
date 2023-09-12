namespace CodingTracker;

class MainMenuController
{
    private readonly MainMenuView view;
    private CodingSessionController? codingSessionController;

    public MainMenuController()
    {
        view = new MainMenuView(this);
    }

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
        // nothing to do
    }
}