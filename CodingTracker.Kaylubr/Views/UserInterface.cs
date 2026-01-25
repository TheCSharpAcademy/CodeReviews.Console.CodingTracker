using Spectre.Console;
using CodingTracker.Controllers;
using CodingTracker.Enums;
using CodingTracker.Utils;

namespace CodingTracker.Views;

internal static class UserInterface
{
    internal static void Run()
    {
        while (true)
        {
            RenderTitle();

            var choice = AnsiConsole.Prompt(
             new SelectionPrompt<MenuChoices>()
             .Title("\n[Green]Pick operation:[/]")
             .HighlightStyle(new Style(Color.Green))
             .AddChoices(Enum.GetValues<MenuChoices>())
            );

            switch (choice)
            {
                case MenuChoices.View:
                    CodingTrackerController.LogAllRecords();
                    break;

                case MenuChoices.Insert:

                    if (!Helper.Confirmation("Proceed with this operation? (No will return to the main menu)"))
                        continue;

                    CodingTrackerController.InsertSession();
                    break;

                case MenuChoices.Update:

                    if (!Helper.Confirmation("Proceed with this operation? (No will return to the main menu)"))
                        continue;

                    CodingTrackerController.UpdateRecord();
                    break;

                case MenuChoices.Delete:

                    if (!Helper.Confirmation("Proceed with this operation? (No will return to the main menu)"))
                        continue;

                    CodingTrackerController.DeleteRecord();
                    break;
                case MenuChoices.Exit:
                    AnsiConsole.WriteLine("\nExiting..");
                    Environment.Exit(0);
                    break;
            }
        }
    }

    static void RenderTitle()
    {
        Console.Clear();
        var panel = new Panel("Welcome to Coding Session Tracker!");
        panel.Border(BoxBorder.Heavy);
        AnsiConsole.Write(panel);
    }
}
