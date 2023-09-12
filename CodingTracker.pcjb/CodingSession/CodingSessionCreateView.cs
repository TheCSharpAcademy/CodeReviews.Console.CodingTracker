namespace CodingTracker;

class CodingSessionCreateView
{
    private readonly CodingSessionController controller;
    public CodingSessionCreateView(CodingSessionController controller)
    {
        this.controller = controller;
    }

    public void Show()
    {
        Console.Clear();
        Console.WriteLine("New Coding Session");
        Console.WriteLine($"Session Start Date & Time [{Configuration.DateTimeFormat}]: ");
        var start = Console.ReadLine();
        Console.WriteLine($"Session End Date & Time [{Configuration.DateTimeFormat}]: ");
        var end = Console.ReadLine();
        controller.Create(start, end);
    }
}