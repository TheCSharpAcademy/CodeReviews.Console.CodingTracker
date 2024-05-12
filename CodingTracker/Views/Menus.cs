using CodingTracker.Controllers;
using Spectre.Console;

namespace CodingTracker.Views;

public class Menus
{
    public static string MenuInput()
    {
        var menuInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold blue]\nMAIN MENU\n[/][blue]What would you like to do?[/]")
                .PageSize(5)
                .AddChoices(new[]
                {
                    "Insert a Record", "Update a Record", "Delete a Record",
                    "Access Reporting Menu", "Close the Application"
                }));

        return menuInput;
    }

    public static void ReportMenu()
    {
        var reportMenuInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold blue]\nREPORTING MENU\n[/][blue]What would you like to do?[/]")
                .PageSize(4)
                .AddChoices(new[]
                {
                    "Generate a Full Report", "Generate a Summary Report", 
                    "Generate a Filtered Report", "Return to the Main Menu"
                }));
        
        switch (reportMenuInput)
        {
            case "Generate a Full Report":
                Console.Clear();
                AnsiConsole.WriteLine($"You have chosen to {reportMenuInput}");
                // GenerateFullReport();
                break;
            case "Generate a Summary Report":
                Console.Clear();
                AnsiConsole.WriteLine($"You have chosen to {reportMenuInput}");
                // GenerateSummaryReport();
                break;
            case "Generate a Filtered Report":
                Console.Clear();
                AnsiConsole.WriteLine($"You have chosen to {reportMenuInput}");
                // GenerateFilteredReport();
                break;
            case "Return to the Main Menu":
                Console.Clear();
                AnsiConsole.WriteLine($"You have chosen to {reportMenuInput}");
                UserInput.GetMenuInput();
                break;

        }
    }
}