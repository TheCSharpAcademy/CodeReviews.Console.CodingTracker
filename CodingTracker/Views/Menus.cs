using Spectre.Console;

namespace CodingTracker.Views;

public static class Menus
{
    public static string MenuInput()
    {
        var menuInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold blue on yellow]\n-- MAIN MENU --\n[/][blue on yellow]What would you like to do?[/]")
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
                .Title("[bold blue on yellow]\nREPORTING MENU\n[/][blue on yellow]What would you like to do?[/]")
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
                TableVisualisationEngine.GenerateFullReport(true);
                break;
            case "Generate a Summary Report":
                Console.Clear();
                AnsiConsole.WriteLine($"You have chosen to {reportMenuInput}");
                TableVisualisationEngine.GenerateSummaryReport();
                break;
            case "Generate a Filtered Report":
                Console.Clear();
                AnsiConsole.WriteLine($"You have chosen to {reportMenuInput}");
                TableVisualisationEngine.GenerateFilteredReport();
                break;
            case "Return to the Main Menu":
                Console.Clear();
                AnsiConsole.WriteLine($"You have chosen to {reportMenuInput}");
                AnsiConsole.WriteLine($"\nReturning to main menu...");
                break;
        }
    }
}