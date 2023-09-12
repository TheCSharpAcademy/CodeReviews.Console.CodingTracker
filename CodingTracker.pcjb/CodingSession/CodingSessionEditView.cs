namespace CodingTracker;

using ConsoleTableExt;

class CodingSessionEditView : BaseView
{
    private readonly CodingSessionController controller;
    private readonly CodingSession session;

    public CodingSessionEditView(CodingSessionController controller, CodingSession session)
    {
        this.controller = controller;
        this.session = session;
    }

    public override void Body()
    {
        Console.WriteLine("Edit Coding Session");
        ConsoleTableBuilder.From(new List<CodingSession>{session}).ExportAndWriteLine();
        
        Console.WriteLine($"Date & Time Format: {Configuration.DateTimeFormat}");
        Console.Write($"New Start: ");
        var start = Console.ReadLine();
        Console.Write($"New End  : ");
        var end = Console.ReadLine();

        controller.Update(session.Id, start, end);
    }
}