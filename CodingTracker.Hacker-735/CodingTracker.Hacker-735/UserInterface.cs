using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;
using static CodingTracker.Hacker_735.Enums;

namespace CodingTracker.Hacker_735
{
    internal class UserInterface
    {
        static internal void ChoicePrompt()
        {
            bool loop = true;
            while (loop == true)
            {
                Console.Clear();
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<MenuAction>()
                    .Title("What would you like to do?")
                     .AddChoices(Enum.GetValues<MenuAction>()));
                switch (choice)
                {
                    case MenuAction.Display:
                        TableController.Display();
                        Console.ReadLine();
                        break;

                    case MenuAction.Insert:
                        TableController.Insert();
                        Console.ReadLine();
                        break;

                    case MenuAction.Delete:
                        TableController.Delete();
                        Console.ReadLine();
                        break;

                    case MenuAction.Update:
                        TableController.Update();
                        Console.ReadLine();
                        break;
                    case MenuAction.Close:
                        loop = false;
                        AnsiConsole.Markup("[red]Bye bye![/]");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}
