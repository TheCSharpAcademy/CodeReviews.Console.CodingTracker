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
    Console.WriteLine("Adding Session.");


    menu.GetKeyReturnMenu();
}