using DatabaseLibrary;
using Spectre.Console;
using CodingTracker.ukpagrace;

class CodingController
{
    readonly Database database = new();
    readonly Goal goal = new();
    static void Main()
    {
        TrackerController trackerController = new ();
        GoalController goalController = new();
        trackerController.CreateTable();
        goalController.CreateGoalTable();
        AnsiConsole.Write(
            new FigletText("CODING TRACKER")
            .LeftJustified()
            .Color(Color.Red3_1)
            );
        bool endApp = false;
        while (!endApp)
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                 .Title("[blueviolet] What's option would you like to perform[/]?")
                 .PageSize(6)
                 .MoreChoicesText("[yellow](Move up and down to reveal more options)[/]")
                 .AddChoices(new[] {
                    "Insert", "Update", "List",
                    "Delete", "LiveCodingSession", "FilterRecords",
                    "GetReport", "SetGoal", "SeeGoalProgress","Quit",
                 })
            );

            switch (option.ToLower())
            {
                case "insert":
                    trackerController.InsertRecord();
                    break;
                case "update":
                    trackerController.UpdateRecord();
                    break;
                case "list":
                    trackerController.ListRecords();
                    break;
                case "delete":
                    trackerController.DeleteRecord();
                    break;
                case "livecodingsession":
                    trackerController.LiveCodingSession();
                    break;
                case "filterrecords":
                    trackerController.FilterRecords();
                    break;
                case "getreport":
                    trackerController.GetReport();
                    break;
                case "setgoal":
                    goalController.SetGoal();
                    break;
                case "seegoalprogress":
                    goalController.SeeGoalProgress();
                    break;
                case "quit":
                    Console.WriteLine("Goodbye\n");
                    endApp = true;
                    break;
                default:
                    Console.WriteLine("Invalid option, please select an option from the menu");
                    break;
            }
        }
    }
}
