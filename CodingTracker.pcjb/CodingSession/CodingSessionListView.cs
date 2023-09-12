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

        Console.WriteLine("Enter ID and press enter to edit/delete a session or press enter alone to return to main menu.");
        var rawInput = Console.ReadLine();
        if (String.IsNullOrEmpty(rawInput)) {
            controller.BackToMainMenu();
        } 
        else if (!long.TryParse(rawInput, out long id))
        {
            controller.ShowList();
        }
        else
        {
            controller.ShowEditDelete(id);
        }
        
    }
}