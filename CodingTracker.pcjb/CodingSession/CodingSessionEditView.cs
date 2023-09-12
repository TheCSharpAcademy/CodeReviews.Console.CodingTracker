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
        
        Console.WriteLine($"New Session Start Date & Time [{Configuration.DateTimeFormat}]: ");
        var start = Console.ReadLine();
        Console.WriteLine($"New Session End Date & Time [{Configuration.DateTimeFormat}]: ");
        var end = Console.ReadLine();

        DateTime.TryParseExact(start, Configuration.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedStart);
        DateTime.TryParseExact(end, Configuration.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedEnd);
        controller.Update(new CodingSession(session.Id, parsedStart, parsedEnd));
    }
}