using Spectre.Console;

namespace CodingTracker.ConsoleInteraction
{
    public class UserInteraction : IUserInteraction
    {
        public void DisplayMenu()
        {
            AnsiConsole.Write(new Markup("\n\nMain Menu"));
            AnsiConsole.Write(new Markup("\nWhat would you like to do?"));
            AnsiConsole.Write(new Markup("\n\n[DarkRed]Type 0 to Close Application[/]"));
            AnsiConsole.Write(new Markup("\n[Green]Type 1 to View All Coding Sessions[/]"));
            AnsiConsole.Write(new Markup("\n[Yellow]Type 2 to Insert New Coding Session[/]"));
            AnsiConsole.Write(new Markup("\n[DarkGreen]Type 3 to Update Coding Session[/]"));
            AnsiConsole.Write(new Markup("\n[Red]Type 4 to Delete Coding Session[/]"));
            AnsiConsole.Write(new Markup("\n[Green]Type 5 to Start Live Coding Session[/]"));
            AnsiConsole.Write(new Markup("\n[Blue]Type 6 to Filter by Period[/]"));
            AnsiConsole.Write(new Markup("\n----------------------------------------\n"));
            AnsiConsole.Write(new Markup("Input: "));
        }


        public string GetUserInput()
        {
            return Console.ReadLine() ?? "";
        }

        public void ShowMessage(string message)
        {
            AnsiConsole.Write(new Markup(message));
        }

        public void ShowMessage(Table table)
        {
            AnsiConsole.Write(table);
        }

        public void ShowMessageTimeout(string message)
        {
            Thread.Sleep(500);
            AnsiConsole.Write(new Markup(message));
            Thread.Sleep(500);
        }

        public void ClearConsole()
        {
            AnsiConsole.Clear();
        }
    }
}