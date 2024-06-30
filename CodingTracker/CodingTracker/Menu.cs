using Spectre.Console;
namespace CodingTracker
{
    public class Menu
    {
        DbController dbController;
        public void SetDbController(DbController _dbController)
        {
            this.dbController = _dbController; 
        }
        public void DisplayMenu()
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
        public void MenuOption(char option) 
        {
            switch (option)
            {
                case '1':
                    dbController.ReadRecords();
                    break;
                case '2':
                    dbController.InsertRecords();
                    break;
                case '3':
                    dbController.DeleteRecord();
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
        public void QuitApplication()
        {
            Environment.Exit(2);
        }
    }
}
