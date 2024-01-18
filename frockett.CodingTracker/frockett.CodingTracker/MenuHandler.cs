using Spectre.Console;

namespace frockett.CodingTracker
{
    internal class MenuHandler
    {
        public void ShowMainMenu()
        {
            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Which operation would you like to perform?")
                .PageSize(10)
                .MoreChoicesText("Use the arrow and enter keys to select")
                .AddChoices(new[]
                {
                    "1. Insert Coding Session", "2. Modify Coding Session Record", 
                    "3. Delete Coding Session Record", "4. Coding Session Timer", 
                    "5. Generate Reports", "0. Exit Program",
                }));

            int menuSelection = UserChoiceToInt(choice);
            
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
                case 0:
                    break;
            }
            
            
            AnsiConsole.WriteLine(menuSelection);
        }

        private int UserChoiceToInt(string choice)
        {
            int menuSelection = int.Parse(choice.Substring(0, 1));
            return menuSelection;  
        }

        private void HandleInsertRecord() 
        { 
            throw new NotImplementedException(); 
        }

        private void HandleDeleteRecord()
        {
            throw new NotImplementedException();
        }

        private void HandleUpdateRecord()
        {
            throw new NotImplementedException();
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
