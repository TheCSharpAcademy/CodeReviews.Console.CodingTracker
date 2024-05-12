using CodingTracker.Views;
using Spectre.Console;

namespace CodingTracker.Controllers;

public class UserInput
{
    public static void GetMenuInput()
    {
        Console.Clear();
        var closeApp = false;

        while (!closeApp)
        {
            var menuInput = Menus.MenuInput();

            switch (menuInput)
            {
                case "Insert a Record":
                    Console.Clear();
                    AnsiConsole.WriteLine($"You have chosen to {menuInput}");
                    CrudManager.InsertSqlRecord();
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
                    Menus.ReportMenu();
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


    internal static string GetDateInput(string dateInput, string inputType)
    {
        string message =
            $"Please provide the [green]{dateInput} date[/] [blue](yyyy-MM-dd)[/] for your {inputType}. Type 0 to return to the main menu: ";
        var input = AnsiConsole.Ask<string>(message);

        if (input == "0") throw new HelpersValidation.InputZero();

        return HelpersValidation.DateInputValidation(input, message, dateInput, inputType);
    }

    internal static string GetTimeInput(string timeInput)
    {
        string message =
            $"Please provide the [green]{timeInput} time[/] [blue](HH:mm)[/] for your coding session. Type 0 to return to the main menu: ";
        var input = AnsiConsole.Ask<string>(message);

        if (input == "0") throw new HelpersValidation.InputZero();

        return HelpersValidation.TimeInputValidation(input, message, timeInput);
    }
}