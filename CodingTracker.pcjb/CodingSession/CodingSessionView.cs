namespace CodingTracker;

using ConsoleTableExt;

class CodingSessionView
{
    private readonly CodingSessionController controller;
    private readonly CodingSession session;
    
    public CodingSessionView(CodingSessionController controller, CodingSession session)
    {
        this.controller = controller;
        this.session = session;
    }

    public void Show()
    {
        Console.Clear();
        Console.WriteLine("Edit/Delete Coding Session");
        ConsoleTableBuilder.From(new List<CodingSession>{session}).ExportAndWriteLine();
        Console.WriteLine("Enter 'e' to edit or 'd' to delete this session and press enter or press enter alone to cancel.");
        
        switch (Console.ReadLine())
        {
            case "e":
                controller.Edit(session);
                break;
            case "d":
                controller.Delete(session);
                break;
            default:
                controller.ShowList();
                break;
        }
    }
}