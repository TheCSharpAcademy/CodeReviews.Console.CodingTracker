using Spectre.Console;

namespace CodingTracker.ConsoleInteraction
{
    public class UserInteraction : IUserInteraction
    {
        // public void DisplayMenu()
        // {
        //     Console.WriteLine("\n\nMain Menu");
        //     Console.WriteLine("\nWhat would you like to do?");
        //     Console.WriteLine("\nType 0 to Close Application");
        //     Console.WriteLine("Type 1 to View All Coding Sessions");
        //     Console.WriteLine("Type 2 to Insert New Coding Session");
        //     Console.WriteLine("Type 3 to Update Coding Session");
        //     Console.WriteLine("Type 4 to Delete Coding Session");
        //     Console.WriteLine("----------------------------------------\n");
        //     Console.Write("Input: ");
        // }

        public void DisplayMenu()
        {
            AnsiConsole.Write(new Markup("\n\nMain Menu"));
            AnsiConsole.Write(new Markup("\nWhat would you like to do?"));
            AnsiConsole.Write(new Markup("\n\n[DarkRed]Type 0 to Close Application[/]"));
            AnsiConsole.Write(new Markup("\n[Green]Type 1 to View All Coding Sessions[/]"));
            AnsiConsole.Write(new Markup("\n[Yellow]Type 2 to Insert New Coding Session[/]"));
            AnsiConsole.Write(new Markup("\n[DarkGreen]Type 3 to Update Coding Session[/]"));
            AnsiConsole.Write(new Markup("\n[Red]Type 4 to Delete Coding Session[/]"));
            AnsiConsole.Write(new Markup("\n----------------------------------------\n"));
            AnsiConsole.Write(new Markup("Input: "));
        }


        public string GetUserInput()
        {
            return Console.ReadLine() ?? "";
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void ShowMessageTimeout(string message)
        {
            Thread.Sleep(500);
            Console.WriteLine(message);
            Thread.Sleep(500);
        }
    }
}