namespace CodingTracker;

using ConsoleTableExt;

class ReportView : BaseView
{
    private readonly ReportController controller;
    private readonly List<ReportResultTotalAndAverage> data;

    public ReportView(ReportController controller, List<ReportResultTotalAndAverage> data)
    {
        this.controller = controller;
        this.data = data;
    }

    public override void Body()
    {
        Console.WriteLine("Report 'Total and Average'");
        ConsoleTableBuilder.From(data).ExportAndWriteLine();
        Console.WriteLine("Period: 'w' week, 'm' month, 'y' year");
        Console.WriteLine("Press enter alone to return to main menu.");
        var input = Console.ReadLine();
        switch (input)
        {
            case "w":
                controller.TotalAndAverage(ReportPeriod.Week);
                break;
            case "m":
                controller.TotalAndAverage(ReportPeriod.Month);
                break;
            case "y":
                controller.TotalAndAverage(ReportPeriod.Year);
                break;
            default:
                controller.BackToMainMenu();
                break;
        }
    }
}