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
        Console.WriteLine("Session Start Date & Time [yyyy-MM-dd HH:mm]: ");
        var start = Console.ReadLine();
        Console.WriteLine("Session End Date & Time [yyyy-MM-dd HH:mm]: ");
        var end = Console.ReadLine();
        controller.Create(start, end);
    }
}