// My Code Tracking Console Application
// Check more information at GitHub ReadMe file
using CodingReports.HopelessCoding;
using DbHelpers.HopelessCoding;
using Spectre.Console;
using User.HopelessCoding;

namespace CodingTracker;

static class Program
{
    static void Main()
    {
        DatabaseHelpers.InitializeDatabase();

        while (true)
        {
            AnsiConsole.Write(new Markup("[yellow1]MAIN MENU\n\n[/]"));
            var selection = MainMenuSelector();

            switch (selection)
            {
                case "A - Add New Record":
                    UserCommands.AddNewRecord();
                    break;
                case "V - View All Records":
                    UserCommands.ViewRecords();
                    break;
                case "U - Update Record":
                    UserCommands.UpdateRecord();
                    break;
                case "D - Delete Record":
                    UserCommands.DeleteRecord();
                    break;
                case "R - Show Reports":
                    Reports.ReportsMenu();
                    break;
                case "0 - Close Application":
                    AnsiConsole.Write(new Markup($"[yellow1]Application is closing...\n[/]"));
                    Environment.Exit(0);
                    break;
                default:
                    AnsiConsole.Write(new Markup($"[red]\nInvalid input, try again.\n[/]"));
                    Console.WriteLine("----------------------------");
                    break;
            }
        }
    }

    internal static string MainMenuSelector()
    {
        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow1]What would you like to do?[/]")
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .HighlightStyle("olive")
                .AddChoices(new[] {
                "A - Add New Record", "V - View All Records", "U - Update Record",
                "D - Delete Record", "R - Show Reports", "0 - Close Application"
                }));

        return selection;
    }
}