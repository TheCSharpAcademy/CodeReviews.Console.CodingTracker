namespace CodingTrackerApp;

internal class Program
{
    static async Task Main()
    {
        bool openApp = true;
        await Spin.Check(openApp);
    }
}