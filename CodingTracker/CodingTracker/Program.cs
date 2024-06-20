using System;
using Spectre.Console;

namespace CodingTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayMenu();
        }
        static char DisplayMenu()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold]Choose from the options below:[/]");
            AnsiConsole.MarkupLine("1 - [bold yellow]View[/] Records");
            AnsiConsole.MarkupLine("2 - [bold yellow]Add[/] record");
            AnsiConsole.MarkupLine("3 - [bold yellow]Delete[/] record");
            AnsiConsole.MarkupLine("4 - [bold yellow]Update[/] record");
            AnsiConsole.MarkupLine("0 - [bold red]Quit[/] the application");
            string choice = Console.ReadLine();
            return choice[0];
        }
        public void MenuOption(char option)
        {
            switch (option)
            {
                case '1':
                    ViewRecords();
                    break;
                case '2':
                    AddRecords();
                    break;
                case '3':
                    DeleteRecords();
                    break;
                case '4':
                    UpdateRecord();
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

        private void ViewRecords()
        {
            throw new NotImplementedException();
        }

        private void AddRecords()
        {
            throw new NotImplementedException();
        }

        private void DeleteRecords()
        {
            throw new NotImplementedException();
        }

        private void UpdateRecord()
        {
            throw new NotImplementedException();
        }

        private void QuitApplication()
        {
            throw new NotImplementedException();
        }
    }
}
