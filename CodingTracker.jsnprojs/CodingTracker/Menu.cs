using Spectre.Console;

namespace CodingTracker;

internal class Menu
{

    internal static void GetUserInput()
    {
        Console.Clear();
        bool closeApp = false;
        while (closeApp == false)
        {
            DisplayMenu();

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    AnsiConsole.MarkupLine("[green]GoodBye![/]");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    Controller.ShowTable();
                    AnsiConsole.MarkupLine("Press anything to go back to main menu");
                    Console.ReadLine();
                    break;
                case "2":
                    Controller.Insert();
                    break;
                case "3":
                    Controller.Delete();
                    break;
                case "4":
                    Controller.Update();
                    break;
                case "5":
                    Controller.Timer();
                    break;
                default:
                    AnsiConsole.MarkupLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }

    static void DisplayMenu()
    {
        AnsiConsole.Clear();       
        AnsiConsole.MarkupLine("\n\n[blue]MAIN MENU[/]");
        AnsiConsole.MarkupLine("\n[blue]What would you like to do?[/]");
        AnsiConsole.MarkupLine("\n[blue]Type 0 to Close Application.[/]");
        AnsiConsole.MarkupLine("[blue]Type 1 to View All Records.[/]");
        AnsiConsole.MarkupLine("[blue]Type 2 to Insert Record.[/]");
        AnsiConsole.MarkupLine("[blue]Type 3 to Delete Record.[/]");
        AnsiConsole.MarkupLine("[blue]Type 4 to Update Record.[/]");
        AnsiConsole.MarkupLine("[blue]Type 5 to Use a Timer.[/]");
        AnsiConsole.MarkupLine("------------------------------------------\n");
    }

}
