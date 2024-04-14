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
                    .PageSize(5)
                    .AddChoices(new[]
                    {
                        "Insert a Record", "Update a Record", "Delete a Record",
                        "Access Reporting Menu", "Close the Application"
                    }));

            switch (menuInput)
            {
                case "Insert a Record":
                    Console.Clear();
                    AnsiConsole.WriteLine($"You have chosen to {menuInput}");
                    CrudManager.InsertRecord();
                    break;
                case "Update a Record":
                    Console.Clear();
                    AnsiConsole.WriteLine($"You have chosen to {menuInput}");
                    // UpdateRecord();
                    break;
                case "Delete a Record":
                    Console.Clear();
                    AnsiConsole.WriteLine($"You have chosen to {menuInput}");
                    // DeleteRecord();
                    break;
                case "Access Reporting Menu":
                    Console.Clear();
                    AnsiConsole.WriteLine($"You have chosen to {menuInput}");
                    // ReportMenu();
                    break;
                case "Close the Application":
                    Console.Clear();
                    AnsiConsole.Markup(
                        $"[bold red]You have chosen to [/][bold underline red]{menuInput}.[/][bold red] Goodbye![/]");
                    closeApp = true;
                    break;
            }
        }
    }

    internal static string GetDateInput()
    {
        string message =
            "Please provide the [green]start date[/] [blue](yyyy-MM-dd)[/] for your coding session. Type 0 to return to the main menu: ";
        var input = AnsiConsole.Ask<string>(message);

        if (input == "0") throw new Validation.InputZero();

        return Validation.DateInputValidation(input, message);
    }
}