namespace CodingTracker;

class CodingSessionCreateView : BaseView
{
    private readonly CodingSessionController controller;
    
    public CodingSessionCreateView(CodingSessionController controller)
    {
        this.controller = controller;
    }

    public override void Body()
    {
        Console.WriteLine("New Coding Session");
        Console.WriteLine($"Date & Time Format: {Configuration.DateTimeFormat}");
        Console.Write($"Start: ");
        var start = Console.ReadLine();
        Console.Write($"End  : ");
        var end = Console.ReadLine();
        controller.Create(start, end);
    }
}