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

var menu = new Menu(new CodingController());
int option = -1;

while(option != 0)
{
    Console.WriteLine("Welcome to Coding Session. This will be tracking your coding session.");
    menu.GetMenu();
    option = menu.GetMenuSelection();

    switch (option)
    {
        case 0:
            break;
        case 1:
            AddSession();
            break;
        case 2:
            DeleteSession();
            break;
        case 3:
            UpdateSession();
            break;
        case 4:
            ShowAllSessions();
            break;
    }
}

void ShowAllSessions()
{
    Console.Clear();
    Console.WriteLine("Delete a Session");

    menu.GetKeyReturnMenu();
}

void UpdateSession()
{
    Console.Clear();
    Console.WriteLine("Delete a Session");

    menu.GetKeyReturnMenu();
}

    void DeleteSession()
{
    Console.Clear();
    Console.WriteLine("Delete a Session");

    menu.GetKeyReturnMenu();
}

void AddSession()
{
    Console.Clear();
    Console.WriteLine("Adding Session.\n");

    Console.WriteLine("Begin Date.");
    DateOnly beginDate = Input.GetDate();

    Console.WriteLine("Begin Time");
    TimeOnly beginTime = Input.GetTime();

    Console.WriteLine("End Date.");
    DateOnly endDate = Input.GetDate();

    Console.WriteLine("End Time");
    TimeOnly endTime = Input.GetTime();

    if (beginTime > endTime)
        Console.WriteLine("Begin Time has to be less than the end time");
    else
    {
        TimeOnly duration = TimeOnly.FromTimeSpan(endTime - beginTime);
        Console.WriteLine($"Time List : {beginTime}");
        Console.WriteLine($"Time List : {endTime}");
        Console.WriteLine($"Time List : {duration}");
    }


    menu.GetKeyReturnMenu();
}