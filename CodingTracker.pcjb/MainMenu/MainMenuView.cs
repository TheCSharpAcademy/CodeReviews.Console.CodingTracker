namespace CodingTracker;

class MainMenuView : BaseView
{
    private MainMenuController controller;

    public MainMenuView(MainMenuController controller)
    {
        this.controller = controller;
    }

    public override void Body()
    {
        Console.WriteLine("1 - New Coding Session");
        Console.WriteLine("2 - List Coding Sessions");
        Console.WriteLine("3 - Stopwatch");
        Console.WriteLine("4 - Report 'Totals and Average'");
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
            case "3":
                controller.Stopwatch();
                break;
            case "4":
                controller.ReportTotalsAndAverage();
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