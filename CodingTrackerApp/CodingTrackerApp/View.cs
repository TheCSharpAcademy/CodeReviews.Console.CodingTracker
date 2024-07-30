using Spectre.Console;

namespace CodingTrackerApp
{
    public class View
    {
        public static void DisplayMainMenu(out int userInput)
        {
            userInput = 0;
            Console.WriteLine();

            Dictionary<string, int> choices = new Dictionary<string, int>
            {
                { "Exit", 0 },
                { "View Records", 1 },
                { "Insert Record", 2 },
                { "Update Record.", 3},
                { "Delete Record", 4},
                { "View Custom Report", 5}
            };

            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .AddChoices(choices.Keys)
                .HighlightStyle(Style.Parse("red"))
                );
            userInput = choices[choice];
        }
    }
}
