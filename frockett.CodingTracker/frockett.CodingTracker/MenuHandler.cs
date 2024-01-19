using Library;
using Spectre.Console;

namespace frockett.CodingTracker
{
    internal class MenuHandler
    {
        private readonly CodingSessionController codingSessionController;
        private readonly DisplayService displayService;
        
        public MenuHandler(CodingSessionController controller, DisplayService display)
        {
            codingSessionController = controller;
            displayService = display;
        }

        public void ShowMainMenu()
        {
            AnsiConsole.Clear();

            string[] menuOptions =
                    {"Insert Coding Session", "Modify Coding Session Record",
                    "Delete Coding Session Record", "Coding Session Timer",
                    "Generate Reports", "Exit Program",};

            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Which operation would you like to perform? Use [green]arrow[/] and [green]enter[/] keys to make a selection.")
                .PageSize(10)
                .MoreChoicesText("Keep scrolling for more options")
                .AddChoices(menuOptions));

            /* 
             * Before, the menu selection was parsed based on an int.parse of the first character, which was a number. 
             * But having the numbers could confuse the user, since you can't input a number in the menu.
             * So instead, menuSelection is the index in the menu array + 1 (the +1 is for ease of human readability)
             */

            int menuSelection = Array.IndexOf(menuOptions, choice) + 1;
            
            switch (menuSelection)
            {
                case 1:
                    HandleInsertRecord();
                    break;
                case 2:
                    HandleUpdateRecord();
                    break;
                case 3:
                    HandleDeleteRecord();
                    break;
                case 4:
                    HandleStartCodingSession();
                    break;
                case 5:
                    HandleReportSubmenu();
                    break;
                case 6:
                    break;
            }
        }

        private void HandleInsertRecord() 
        {
            codingSessionController.InsertCodingSession();
            ShowMainMenu();
        }

        private void HandleDeleteRecord()
        {
            displayService.PrintSessionList(codingSessionController.FetchAllSessionHistory());
            codingSessionController.DeleteCodingSession();
            ShowMainMenu();
        }

        private void HandleUpdateRecord()
        {
            displayService.PrintSessionList(codingSessionController.FetchAllSessionHistory());
            codingSessionController.UpdateCodingSession();
            ShowMainMenu();
        }

        private void HandleStartCodingSession()
        {
            var stopwatch = displayService.DisplayStopwatch(codingSessionController.StartCodingSession());
            codingSessionController.StopCodingSession(stopwatch);
            AnsiConsole.Markup("\n[green]Session recorded, please press any key to return to the main menu...[/]");
            Console.ReadKey(true);
            Thread.Sleep(100);
            ShowMainMenu();
        }

        private void HandleReportSubmenu()
        {
            // TODO create submenu and options for custom reports
            displayService.PrintSessionList(codingSessionController.FetchAllSessionHistory());
            ShowMainMenu();
        }
    }
}
