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
}