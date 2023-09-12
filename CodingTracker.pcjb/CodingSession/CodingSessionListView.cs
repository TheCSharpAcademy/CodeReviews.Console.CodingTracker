namespace CodingTracker;

using ConsoleTableExt;

class CodingSessionListView
{
    private readonly CodingSessionController controller;
    private readonly List<CodingSession> sessions;
    public CodingSessionListView(CodingSessionController controller, List<CodingSession> sessions)
    {
        this.controller = controller;
        this.sessions = sessions;
    }

    public void Show()
    {
        Console.Clear();
        Console.WriteLine("All Coding Sessions");

        if (sessions != null && sessions.Count > 0)
        {
            ConsoleTableBuilder.From(sessions).ExportAndWriteLine();
        }
        else
        {
            Console.WriteLine("No coding sessions found.");
        }

        Console.WriteLine("Press enter to proceed.");
        Console.ReadLine();
        controller.BackToMainMenu();
    }
}