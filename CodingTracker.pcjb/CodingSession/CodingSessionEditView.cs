namespace CodingTracker;

using System.Globalization;
using ConsoleTableExt;

class CodingSessionEditView
{
    private readonly CodingSessionController controller;
    private readonly CodingSession session;

    public CodingSessionEditView(CodingSessionController controller, CodingSession session)
    {
        this.controller = controller;
        this.session = session;
    }

    public void Show()
    {
        Console.Clear();
        Console.WriteLine("Edit Coding Session");
        ConsoleTableBuilder.From(new List<CodingSession>{session}).ExportAndWriteLine();
        
        Console.WriteLine("New Session Start Date & Time [yyyy-MM-dd HH:mm]: ");
        var start = Console.ReadLine();
        Console.WriteLine("New Session End Date & Time [yyyy-MM-dd HH:mm]: ");
        var end = Console.ReadLine();

        DateTime.TryParseExact(start, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedStart);
        DateTime.TryParseExact(end, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedEnd);
        controller.Update(new CodingSession(session.Id, parsedStart, parsedEnd));
    }
}