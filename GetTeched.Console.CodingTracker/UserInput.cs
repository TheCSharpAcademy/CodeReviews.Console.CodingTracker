using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding_tracker;

public class UserInput
{
    internal void MainMenu()
    {
        bool endApplication = false;
        do
        {
            var crudActions = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select the operation with the arrow keys")
            .PageSize(10)
            .AddChoices(new[]
            {
                "View Sessions", "Enter New Sessions",
                "Update Session", "Delete Sessions", "Exit Application"
            }));

            switch (crudActions)
            {
                case "View Sessions":
                    AnsiConsole.Write(new Markup("[bold red]Not Implemented yet.[/]"));
                    AnsiConsole.WriteLine("\n\nPress any key to return to the Main Menu.");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case "Enter New Sessions":
                    AnsiConsole.WriteLine("\n\nPress any key to return to the Main Menu.");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case "Update Session":
                    AnsiConsole.Write(new Markup("[bold red]Not Implemented yet.[/]"));
                    AnsiConsole.WriteLine("\n\nPress any key to return to the Main Menu.");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case "Delete Sessions":
                    AnsiConsole.Write(new Markup("[bold red]Not Implemented yet.[/]"));
                    AnsiConsole.WriteLine("\n\nPress any key to return to the Main Menu.");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case "Exit Application":
                    endApplication = true;
                    Environment.Exit(0);
                    break;
            }
        } while (!endApplication);
    }
}
