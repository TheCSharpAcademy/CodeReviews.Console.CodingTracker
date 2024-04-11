namespace CodingTracker.Controllers;

using Spectre.Console;

public class UserInput
{
    public static void GetMenuInput()
    {
        Console.Clear();
        var closeApp = false;

        while (!closeApp)
        {
            var menuInput = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold blue]\n\nMAIN MENU\n[/][blue]What would you like to do?[/]")
                    .PageSize(6)
                    .AddChoices(new[]
                    {
                        "View All Records", "Insert a Record", "Update a Record", 
                        "Delete a Records", "Access Reporting Menu", "Close the Application"
                    }));

            switch (menuInput)
            {
                case "View All Records":
                    Console.Clear();
                    AnsiConsole.Markup($"[red]You have chosen to {menuInput}\n[/]");
                    // ViewAllRecords()
                    break;
                case "Close the Application":
                    Console.Clear();
                    AnsiConsole.Markup($"[bold underline red]You have chosen to {menuInput}. Goodbye![/]");
                    closeApp = true;
                    break;
                default:
                    AnsiConsole.Markup($"[yellow]The option you have chosen is not yet implemented. Please select another option.");
                    break;
            }
        }
    }
}