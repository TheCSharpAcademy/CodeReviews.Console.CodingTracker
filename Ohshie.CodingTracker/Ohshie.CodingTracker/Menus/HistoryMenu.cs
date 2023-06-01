namespace Ohshie.CodingTracker.Menus;

internal class HistoryMenu
{
    private readonly SessionsDisplay _display = new();
    private readonly SessionEditor _sessionEditor = new();

    internal void Initialize()
    {
        Console.Clear();
        bool chosenExit = false;
        while (!chosenExit)
        {
            if(!_display.ShowSessions(5)) return;
            
            Console.WriteLine("Choose menu item by pressing corresponding #:\n");
            Console.WriteLine("1. Show/edit all previous coding sessions\n" +
                              "2. Wipe previous data\n" +
                              "3. Go back");
            
            var keyPressed = Console.ReadKey(true).Key;
            
            switch (keyPressed)
            {
                case ConsoleKey.D1:
                {
                    ChooseSessionMenu chooseSessionMenu = new();
                    chooseSessionMenu.Initialize();
                    break;
                }
                case ConsoleKey.D2:
                {
                    if (ConfirmDeletion()) _sessionEditor.WipeData();
                    break;
                }
                case ConsoleKey.D3:
                {
                    chosenExit = true;
                    break;
                }
                default:
                {
                    Console.Clear();
                    Console.WriteLine("Looks like you pressed something that you shouldn't. Let's try again.\n");
                    continue;
                }
            }  
        }
    }

    private bool ConfirmDeletion()
    {
        bool userConfirmed = false;
        while (!userConfirmed)
        {
            Console.Clear();
            Console.WriteLine("Type yes to confirm wiping of all previous sessions\n");
            Console.WriteLine("Type no or just press enter to go back");

            string? userInput = Console.ReadLine();
            
            if (Verify.GoBack(userInput))  return false;
            if (Verify.Confirm(userInput))  userConfirmed = true;
        }

        return true;
    }
}