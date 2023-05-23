using CodingTracker.barakisbrown;
using Serilog;

// Note : Following will make sure the only way to app will exit is via CRTL|BREAK
Console.CancelKeyPress += delegate (object? sender, ConsoleCancelEventArgs e)
{
    e.Cancel = true;
};
// Initialize SeriLog for File Use
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("app.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// I have to do this for it pass the test
#pragma warning disable S1481 // Unused local variables should be removed
Menu menu = new Menu(new CodingController(), new CodingSession());
#pragma warning restore S1481 // Unused local variables should be removed
