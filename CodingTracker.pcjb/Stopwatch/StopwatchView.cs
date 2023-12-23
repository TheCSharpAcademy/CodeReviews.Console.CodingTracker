namespace CodingTracker;

class StopwatchView : BaseView
{
    private readonly StopwatchController controller;

    private readonly Stopwatch stopwatch;

    public StopwatchView(StopwatchController controller, Stopwatch stopwatch)
    {
        this.controller = controller;
        this.stopwatch = stopwatch;
    }

    public override void Body()
    {
        if (stopwatch.IsRunning())
        {
            Console.WriteLine($"Stopwatch is running.");
            Console.WriteLine($"Started at: {stopwatch.GetStart()}.");
            Console.WriteLine("Enter 's' and press enter to stop the stopwatch and save the session.");
            Console.WriteLine("Press enter alone to return to main menu.");
            switch (Console.ReadLine())
            {
                case "s":
                    controller.Stop();
                    break;
                default:
                    controller.BackToMainMenu();
                    break;
            }
        }
        else
        {
            Console.WriteLine("Stopwatch not started yet.");
            Console.WriteLine("Enter 's' and press enter to start the stopwatch.");
            Console.WriteLine("Press enter alone to return to main menu.");
            switch (Console.ReadLine())
            {
                case "s":
                    controller.Start();
                    break;
                default:
                    controller.BackToMainMenu();
                    break;
            }
        }
    }
}