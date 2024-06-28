using System.Globalization;
using DatabaseLibrary;
using Application.Entities;
using Spectre.Console;
using CodingTracker.ukpagrace;

class CodingController
{
    readonly Database database = new();
    readonly Goal goal = new();
    bool stop = false;
    static void Main()
    {
        TrackerController trackerController = new ();
        trackerController.CreateTable();
       // controller.CreateGoalTable();
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
                 .Title("What's [blue]option would you like to perform[/]?")
                 .PageSize(6)
                 .MoreChoicesText("[yellow](Move up and down to reveal more options)[/]")
                 .AddChoices(new[] {
                    "Insert", "Update", "List",
                    "Delete", "StopWatch", "FilterRecords",
                    "GetReport", "SetGoal", "SeeGoalProgress","Quit",
                 })
            );

            switch (option)
            {
                case "Insert":
                    trackerController.InsertTable();
                    break;
                case "0":
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
