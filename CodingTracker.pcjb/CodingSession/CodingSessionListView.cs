namespace CodingTracker;

using ConsoleTableExt;

class CodingSessionListView : BaseView
{
    private readonly CodingSessionController controller;
    private readonly List<CodingSession> sessions;

    public CodingSessionListView(CodingSessionController controller, List<CodingSession> sessions)
    {
        this.controller = controller;
        this.sessions = sessions;
    }

    public override void Body()
    {
        Console.WriteLine("All Coding Sessions");

        if (sessions != null && sessions.Count > 0)
        {
            ConsoleTableBuilder.From(sessions).ExportAndWriteLine();
        }
        else
        {
            Console.WriteLine("No coding sessions found.");
        }

        Console.WriteLine("Order: '+' ascending, '-' decending");
        Console.WriteLine("Filter: 'x' last 7 days, 'y' last 4 weeks, 'z' last 12 months, 'n' no filter");
        Console.WriteLine("Enter ID and press enter to edit/delete a session.");
        Console.WriteLine("Press enter alone to return to main menu.");
        var rawInput = Console.ReadLine();
        if (String.IsNullOrEmpty(rawInput))
        {
            controller.BackToMainMenu();
        }
        else if (!long.TryParse(rawInput, out long id))
        {
            switch (rawInput)
            {
                case "+":
                    controller.SetSortOrder(SortOrder.Ascending);
                    break;
                case "-":
                    controller.SetSortOrder(SortOrder.Descending);
                    break;
                case "x":
                    controller.SetFilterPeriod(FilterPeriod.LastSevenDays);
                    break;
                case "y":
                    controller.SetFilterPeriod(FilterPeriod.LastFourWeeks);
                    break;
                case "z":
                    controller.SetFilterPeriod(FilterPeriod.LastTwelveMonths);
                    break;
                case "n":
                    controller.SetFilterPeriod(FilterPeriod.None);
                    break;
            }
            controller.ShowList();
        }
        else
        {
            controller.ShowEditDelete(id);
        }
    }
}