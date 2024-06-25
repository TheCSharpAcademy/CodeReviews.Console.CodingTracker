using System;
using Spectre.Console;

namespace CodingTracker
{
    class Program
    {
        DbController dbController = new DbController();
        static void Main(string[] args)
        {
            DisplayMenu();     
        }
        static void DisplayMenu()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold]Choose from the options below:[/]");
            AnsiConsole.MarkupLine("1 - [bold yellow]View[/] Records");
            AnsiConsole.MarkupLine("2 - [bold yellow]Add[/] record");
            AnsiConsole.MarkupLine("3 - [bold yellow]Delete[/] record");
            AnsiConsole.MarkupLine("4 - [bold yellow]Update[/] record");
            AnsiConsole.MarkupLine("0 - [bold red]Quit[/] the application");
            string choice = Console.ReadLine();
            MenuOption(choice[0]);
        }
        public static void MenuOption(char option) //static for now, will update later. Testing
        {
            DbController dbController = new DbController();
            switch (option)
            {
                case '1':
                    dbController.ReadRecords();
                    break;
                case '2':
                    dbController.InsertRecords();
                    break;
                case '3':
                    DeleteRecords();
                    break;
                case '4':
                    dbController.UpdateRecord();
                    break;
                case '0':
                    QuitApplication();
                    break;
                default:
                    AnsiConsole.MarkupLine("Please try again!");
                    DisplayMenu();
                    break;
            }
        }

        private static void DeleteRecords()
        {
            throw new NotImplementedException("Not yet implemented.");
        }

        private static void QuitApplication()
        {
            throw new NotImplementedException();
        }
    }
}
