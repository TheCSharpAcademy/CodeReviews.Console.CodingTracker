using Spectre.Console;
using static CodingTracker.Enums;

namespace CodingTracker
{
    internal class Menu
    {
        internal void Show()
        {
            bool closeApp = false;
            Controller controller = new();
            AnsiConsole.Clear();
            do
            {
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<MenuOptions>()
                    .Title("What would you like to do?")
                    .AddChoices(Enum.GetValues<MenuOptions>())
                    .UseConverter(option => string.Concat(option.ToString()
                    .Select((c, i) => i > 0 && char.IsUpper(c) ? " " + c : c.ToString()))));

                switch (option)
                {
                    case MenuOptions.ViewAllSessions:
                        controller.ViewAllSession();
                        break;
                    case MenuOptions.AddSession:
                        controller.AddSession();
                        break;
                    case MenuOptions.UpdateSession:
                        controller.UpdateSession();
                        break;
                    case MenuOptions.DeleteSession:
                        controller.DeleteSession();
                        break;
                    case MenuOptions.Exit:
                        AnsiConsole.MarkupLine("[bold yellow]Goodbye![/]");
                        Environment.Exit(0);
                        break;
                }
            } while (closeApp == false);
        }
    }
}
