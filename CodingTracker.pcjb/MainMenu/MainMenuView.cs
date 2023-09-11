namespace CodingTracker;

class MainMenuView
{
    private MainMenuController controller;

    public MainMenuView(MainMenuController controller)
    {
        this.controller = controller;
    }

    public void Show()
    {
        Console.Clear();
        Console.WriteLine("1 - New Coding Session");
        Console.WriteLine("2 - List Coding Sessions");
        Console.WriteLine("0 - Exit");
        Console.WriteLine("Enter one of the numbers above to select a menu option.");

        switch (Console.ReadKey().KeyChar.ToString())
        {
            case "1":
                controller.NewCodingSession();
                break;
            case "2":
                controller.ListCodingSessions();
                break;
            case "0":
                controller.Exit();
                break;
            default:
                controller.ShowMainMenu();
                break;
        }
    }
}