namespace CodingTracker;

using ConsoleTableExt;

class CodingSessionView : BaseView
{
    private readonly CodingSessionController controller;
    private readonly CodingSession session;
    
    public CodingSessionView(CodingSessionController controller, CodingSession session)
    {
        this.controller = controller;
        this.session = session;
    }

    public override void Body()
    {
        Console.WriteLine("Edit/Delete Coding Session");
        ConsoleTableBuilder.From(new List<CodingSession>{session}).ExportAndWriteLine();
        Console.WriteLine("Enter 'e' to edit or 'd' to delete this session and press enter.");
        Console.WriteLine("Press enter alone to return to list.");
        
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