using Spectre.Console;
using System.Reflection.Metadata.Ecma335;

namespace coding_tracker;

public class UserInput
{
    public SessionController SessionController { get; set; }

    public UserInput(SessionController sessionController)
    {
        SessionController = sessionController;
        sessionController.UserInput = this;
    }

    static InputValidation validation = new();
    static RandomSeed randomSeed = new();
    internal void MainMenu()
    {
        bool endApplication = false;
        do
        {
            var crudActions = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select the operation with the arrow keys")
            .PageSize(10)
            .AddChoices(new[]
            {
                "View Sessions", "Enter New Sessions", "Reports",
                "Update Session", "Delete Sessions", "Exit Application",
                "Random Seed Database", "Testing Print"
            }));

            switch (crudActions)
            {
                case "View Sessions":
                    SessionController.ViewAllSessions();
                    Console.ReadLine();
                    AnsiConsole.Clear();
                    break;
                case "Enter New Sessions":
                    SessionTrackingType();
                    break;
                case "Reports":
                    ReportSelection();
                    break;
                case "Update Session":
                    SessionController.UpdateSession();
                    AnsiConsole.Clear();
                    break;
                case "Delete Sessions":
                    SessionController.DeleteSession();
                    AnsiConsole.Clear();
                    break;
                case "Exit Application":
                    endApplication = true;
                    Environment.Exit(0);
                    break;
                case "Random Seed Database":
                    randomSeed.GenerateRandomData();
                    break;
                case "Testing Print":
                    TestingPrint();
                    break;
            }
        } while (!endApplication);
    }
    internal void SessionTrackingType()
    {
        var trackingType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select the operation with the arrow keys")
            .PageSize(10)
            .AddChoices(new[]
            {
                "Stopwatch Session", "Manual Session Entry", "Back to Main Menu"
            }));

        switch (trackingType)
        {
            case "Stopwatch Session":
                SessionController.SessionStopwatch();
                break;
            case "Manual Session Entry":
                SessionController.AddNewManualSession();
                break;
            case "Back to Main Menu":
                break;
        }
    }

    internal void ReportSelection()
    {
        var reportType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select the report with the arrow keys")
            .PageSize(10)
            .AddChoices(new[]
            {
                "Date Range Report","Biweekly Report", "12 Month Report",
                "Return To Main Menu"
            }));

        switch (reportType)
        {
            case "Date Range Report":
                SessionController.GetDateRange();
                break;
            case "Biweekly Report":
                SessionController.GetBiweeklyRange();
                break;
            case "12 Month Report":
                SessionController.GetYearToDateRange();
                break;
            case "Return To Main Menu":
                break;
        }
    }

    internal string[] Sorting()
    {
        string[] sortingTypes = new string[2];
        var sortType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select the sorting method with arrow keys")
            .PageSize(10)
            .AddChoices(new[]
            {
                "Duration Ascending", "Duration Descending", "Date Ascending",
                "Date Descending", "Return to Main Menu"
            }));

        switch (sortType)
        {
            case "Duration Ascending":
                sortingTypes[0] = "Duration";
                sortingTypes[1] = "ASC";
                break;
            case "Duration Descending":
                sortingTypes[0] = "Duration";
                sortingTypes[1] = "DESC";
                break;
            case "Date Ascending":
                sortingTypes[0] = "Date";
                sortingTypes[1] = "ASC";
                break;
            case "Date Descending":
                sortingTypes[0] = "Date";
                sortingTypes[1] = "DESC";
                break;
            case "Return to Main Menu":
                break;
        }
        return sortingTypes;
    }

    internal void TestingPrint()
    {
        
        //var runningProcessByName = Process.GetProcessesByName("Microsoft Visual Studio 2022");
        //if (runningProcessByName.Length < 1)
        //{
        //    stopwatch.Stop();
        //}
        //else
        //{
        //    stopwatch.Start();
        //}

        //Console.WriteLine(ApplicationIsActivated());

    }

    //public static bool ApplicationIsActivated()
    //{
    //    var activatedHandle = GetForegroundWindow();
    //    if (activatedHandle == IntPtr.Zero)
    //    {
    //        return false;       // No window is currently activated
    //    }

    //    var procId = Process.GetCurrentProcess().Id;
    //    int activeProcId;
    //    GetWindowThreadProcessId(activatedHandle, out activeProcId);

    //    return activeProcId == procId;
    //}


    //[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    //private static extern IntPtr GetForegroundWindow();

    //[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    //private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
}
