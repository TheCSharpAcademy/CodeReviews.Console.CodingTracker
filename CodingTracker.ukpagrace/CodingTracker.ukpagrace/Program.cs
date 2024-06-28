using System.Globalization;
using DatabaseLibrary;
using Application.Entities;
using Spectre.Console;

class CodingController
{
    readonly Database database = new();
    readonly Goal goal = new();
    bool stop = false;
    static void Main()
    {
        CodingController controller = new CodingController();
        controller.CreateTable();
        controller.CreateGoalTable();
        AnsiConsole.Write(
            new FigletText("CODING TRACKER")
            .LeftJustified()
            .Color(Color.Red3_1)
            );
        bool endApp = false;
        while (!endApp)
        {
            Console.WriteLine("Select an option");
            Console.WriteLine("1 - Insert");
            Console.WriteLine("2 - Update");
            Console.WriteLine("3 - List");
            Console.WriteLine("4 - Delete");
            Console.WriteLine("5 - StopWatch");
            Console.WriteLine("6 - Filter records");
            Console.WriteLine("7 - Get Report");
            Console.WriteLine("8 - Set A goal");
            Console.WriteLine("9 - See Goal progress");
            Console.WriteLine("0 - Quit");

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What's [green]option would you like to perform[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                    .AddChoices(new[] {
                        "Insert", "Update", "List",
                        "Delete", "StopWatch", "FilterRecords",
                        "GetReport", "SetGoal", "SeeGoalProgress","Quit",
                    }));
            switch (option)
            {
                case "1":
                    controller.InsertTable();
                    break;
                case "2":
                    controller.UpdateTable();
                    break;
                case "3":
                    controller.ListTable();
                    break;
                case "4":
                    controller.DeleteTable();
                    break;
                case "5":
                    controller.StartWatch();
                    break;
                case "6":
                    controller.FilterRecords();
                    break;
                case "7":
                    controller.GetReport();
                    break;
                case "8":
                    controller.SetGoal();
                    break;
                case "9":
                    controller.SeeGoalProgress();
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
