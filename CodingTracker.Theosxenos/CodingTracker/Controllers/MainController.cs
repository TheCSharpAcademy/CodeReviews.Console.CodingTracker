namespace CodingTracker.Controllers;

public class MainController
{
    public void ShowMainMenu()
    {
        var exitMenu = false;
        var view = new BaseView();
        var sessionController = new SessionController();
        var sessionLogController = new SessionLogController();


        Dictionary<string, Action> menuItems = new()
        {
            { "Start Session", sessionController.StartSession },
            { "Log Session", sessionLogController.CreateSessionLog },
            { "List Sessions", sessionController.ListSessions },
            { "Manage Sessions", sessionController.ManageSessions },
            { "Manage Logs", sessionLogController.ManageLogs },
            { "Exit", () => exitMenu = true }
        };
        do
        {
            var choice = view.ShowMenu(menuItems.Keys);
            menuItems[choice].Invoke();
        } while (!exitMenu);
    }
}