using Spectre.Console;

internal class InputHelper
{
    internal static int GetPositiveNumberInput(string message)
    {
        var input = AnsiConsole.Ask<int>(message);
        while (input <= 0)
        {
            AnsiConsole.Markup("[red]Invalid input. Only positive numbers accepted.[/]\n");
            input = AnsiConsole.Ask<int>("Enter a valid number:");
        }
        return input;
    }
}
