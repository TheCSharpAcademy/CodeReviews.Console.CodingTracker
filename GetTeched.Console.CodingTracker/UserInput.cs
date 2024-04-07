using Spectre.Console;

namespace coding_tracker;

public class UserInput
{
    public SessionController SessionController { get; set; }


    public UserInput(SessionController sessionController)
    {
        SessionController = sessionController;
        sessionController.UserInput = this;
    }
    internal void Testing(bool testMode)
    {
        if (testMode)
        {
            if (SessionController.GetIds().Length == 0)
            {
                RandomSeed randomSeed = new();
                randomSeed.GenerateRandomData();
            }

        }
    }
    internal void GoalMenu()
    {
        if (SessionController.CodingGoalCreated() == true)
        {
            AnsiConsole.Write(new FigletText("No Coding Goal Found").Color(Color.Teal));
            AnsiConsole.Write(new Markup("\n[red]Press any key to enter a coding goal[/]"));
            Console.ReadLine();
            SessionController.CodingGoalInsert();
        }
        SessionController.CodingGoalReview();
        return;
    }
    internal void MainMenu()
    {
        bool endApplication = false;
        do
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Coding Tracker").Color(Color.Teal));

            var crudActions = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("\n Please select the operation with the arrow keys")
            .PageSize(10)
            .AddChoices(new[]
            {
                "View Sessions", "View Current Goal", "Enter New Sessions", "Reports",
                "Update Session", "Delete Sessions", "Exit Application"
            }));

            switch (crudActions)
            {
                case "View Sessions":
                    SessionController.ViewAllSessions();
                    Console.ReadLine();
                    break;
                case "View Current Goal":
                    SessionController.CodingGoalReview();
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
            }
        } while (!endApplication);
    }
    internal void SessionTrackingType()
    {
        var trackingType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("\nPlease select the operation with the arrow keys")
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
            .Title("\nPlease select the report with the arrow keys")
            .PageSize(10)
            .AddChoices(new[]
            {
                "View Coding Goals","Date Range Report","Biweekly Report", "12 Month Report",
                "Return To Main Menu"
            }));

        switch (reportType)
        {
            case "View Coding Goals":
                SessionController.ViewCodingGoals();
                Console.ReadLine();
                break;
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
}
