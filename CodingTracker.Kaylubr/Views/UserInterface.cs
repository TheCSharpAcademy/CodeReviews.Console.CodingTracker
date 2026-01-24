using Spectre.Console;
using CodingTracker.Enums;

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
             .Title("[Chartreuse3_1]Pick operation:[/]")
             .HighlightStyle(new Style(Color.Chartreuse3_1))
             .AddChoices(Enum.GetValues<MenuChoices>())
           );

            switch (choice)
            {
                case MenuChoices.View:
                    break;
                case MenuChoices.Insert:
                    break;
                case MenuChoices.Update:
                    break;
                case MenuChoices.Delete:
                    break;
            }

            Console.ReadKey();
        }
    }

    static void RenderTitle()
    {
        Console.Clear();
        var panel = new Panel("Welcome to Coding Session Tracker!");
        panel.Border(BoxBorder.Ascii);
        AnsiConsole.Write(panel);
    }
}
