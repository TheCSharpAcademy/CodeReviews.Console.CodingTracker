using Spectre.Console;

namespace CodingTracker
{
    public class UserInterface
    {
        internal static void MainMenu(CodingController codingController)
        {
            while (true)
            {
                AnsiConsole.Clear();
                FigletText title = new("Coding Tracker");
                AnsiConsole.Write(title);
                Enums.MenuOption choice = AnsiConsole.Prompt(
                        new SelectionPrompt<Enums.MenuOption>()
                        .Title("what do?")
                        .PageSize(10)
                        .MoreChoicesText("move up or down to choose")
                        .AddChoices(Enum.GetValues<Enums.MenuOption>())
                        );

                AnsiConsole.WriteLine($"{choice}");

                switch (choice)
                {
                    case Enums.MenuOption.Create:
                        codingController.Create();
                        break;
                    case Enums.MenuOption.Read:
                        codingController.Read();
                        break;
                    case Enums.MenuOption.Update:
                        codingController.Update();
                        break;
                    case Enums.MenuOption.Delete:
                        codingController.Delete();
                        break;
                    case Enums.MenuOption.Start:
                        codingController.Start();
                        break;
                    case Enums.MenuOption.Report:
                        codingController.Report();
                        break;
                    case Enums.MenuOption.Goal:
                        codingController.Goal();
                        break;
                    case Enums.MenuOption.Exit:
                        AnsiConsole.WriteLine("Exiting Program");
                        return;
                    default:
                        AnsiConsole.WriteLine("invalid");
                        break;
                }
                AnsiConsole.MarkupLine("press any key to continue");
                _ = Console.ReadKey();
            }
        }
    }
}
