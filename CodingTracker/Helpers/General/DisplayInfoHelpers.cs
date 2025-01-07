using Spectre.Console;

internal class DisplayInfoHelpers
{
    public static readonly string Back = "Back";

    internal static void PressAnyKeyToContinue()
    {
        AnsiConsole.Status()
            .Start("[yellow]Press any key to continue[/]", ctx =>
            {
                ctx.Spinner(Spinner.Known.Star);
                ctx.SpinnerStyle(Style.Parse("yellow"));
                Console.ReadKey(true);
            });
        Console.Clear();
    }

    internal static bool ConfirmDeletion()
    {
        var confirm = GetYesNoAnswer("[red]Are you sure?[/]");
        return confirm;
    }

    internal static bool GetYesNoAnswer(string message)
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(message)
                .PageSize(10)
                .AddChoices("Yes")
                .AddChoices("No")
            );
        return (choice == "Yes");
    }

    internal static string GetChoiceFromSelectionPrompt(string message, IEnumerable<string> choices)
    {
        var selectedChoice = AnsiConsole.Prompt(
               new SelectionPrompt<string>()
                   .Title(message)
                   .PageSize(10)
                   .AddChoices(Back)
                   .AddChoices(choices)
               );
        return selectedChoice;
    }
}
