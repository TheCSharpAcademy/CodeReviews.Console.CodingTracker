using Library;
using Spectre.Console;

namespace frockett.CodingTracker
{
    internal class MenuHandler
    {
        private readonly CodingSessionController codingSessionController;
        
        public MenuHandler(CodingSessionController controller)
        {
            codingSessionController = controller;
        }

        public void ShowMainMenu()
        {
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
                    //HandleStartCodingSession();
                    break;
                case 5:
                    //HandleReportSubmenu();
                    break;
                case 6:
                    break;
            }
        }

        private void HandleInsertRecord() 
        {
            codingSessionController.InsertCodingSession();
        }

        private void HandleDeleteRecord()
        {
            codingSessionController.DeleteCodingSession();
        }

        private void HandleUpdateRecord()
        {
            codingSessionController.UpdateCodingSession();
        }

        private void HandleStartCodingSession()
        {
            throw new NotImplementedException();
        }

        private void HandleReportSubmenu()
        {
            throw new NotImplementedException();
        }
    }
}
