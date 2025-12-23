using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Rendering;
using static CodingTracker.Enums;
namespace CodingTracker
{
    internal class UserInterface
    {
        public static Style MenuStyle = new Style(
        foreground: Color.Green,
        decoration: Decoration.Bold
        );
        internal static void MainMenu()
        {
            TimeController timeController = new();
            timeController.CreateDB();
            AnsiConsole.Write(new Align(
                              new Markup("Welcome to the Coding Tracker!\n", UserInterface.MenuStyle),
                              HorizontalAlignment.Center));

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuChoice>()
                .Title("[green bold]What would you like to do?[/]")
                .AddChoices(Enum.GetValues<MenuChoice>())
                );

            switch (choice)
            {
                case MenuChoice.StartTime:
                    timeController.StartTime();
                    break;

                case MenuChoice.InsertTime:
                    timeController.InsertTime();
                    break;

                case MenuChoice.CheckHistory:
                    timeController.ViewHistory();
                    break;

                case MenuChoice.DeleteSession:
                    timeController.DeleteSession();
                    break;

                case MenuChoice.DeleteHistory:
                    timeController.DeleteHistory();
                    break;

                case MenuChoice.Exit:
                    AnsiConsole.Write(new Markup("Closing the Coding Tracker\n", MenuStyle));
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
