using System.Globalization;
using Spectre.Console;

public class Validation
{
    public static string GetDateInput(string message1, string message2, string message3)
    {
        var dateInput = AnsiConsole.Prompt(
            new TextPrompt<string>($"{message1} [green]{message2}[/] {message3}: ")
                .PromptStyle("blue")
                .AllowEmpty());

        if (dateInput == "0") UserInput.GetUserInput();

        while (!DateTime.TryParseExact(dateInput,"dd-MM-yy HH:mm",new CultureInfo("en-US"),DateTimeStyles.None, out _))
        {
            dateInput = AnsiConsole.Prompt(
                new TextPrompt<string>(
                        $"[red]Invalid format. Use the following: [/][green](Format dd-mm-yy HH:mm)[/] Or type 0 to return to main menu: ")
                    .PromptStyle("blue")
                    .AllowEmpty());
            if(dateInput == "0") UserInput.GetUserInput();
        }
        return dateInput;
    }
}