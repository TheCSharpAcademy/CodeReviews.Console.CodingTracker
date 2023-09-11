namespace CodingTracker;

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
            var format = "{0,5} {1,-20} {2,-20} {3,15}";
            Console.WriteLine(String.Format(format, "ID", "Start", "End", "Duration"));
            foreach (CodingSession session in sessions)
            {
                Console.WriteLine(String.Format(format, session.Id, session.StartTime, session.EndTime, session.Duration));
            }
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