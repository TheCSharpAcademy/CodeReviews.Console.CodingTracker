namespace Ohshie.CodingTracker.Menus;

public class MainMenu
{
    public void Initialize()
    {
        Console.WriteLine("--------Hey! Time to put your head down and code.--------\n");
        
        bool chosenExit = false;
        while (!chosenExit)
        {
            Console.WriteLine("Choose menu item by pressing corresponding #:\n");
                  
            Console.WriteLine("1. New coding tracker\n" +
                              "2. Show previous coding sessions\n" +
                              "3. Exit");
                  
            var keyPressed =Console.ReadKey(true).Key;
            
            switch (keyPressed)
            {
                case ConsoleKey.D1:
                {
                    NewCodingSession session = new NewCodingSession();
                    session.NewSession();
                    break;
                }
                case ConsoleKey.D2:
                {
                    HistoryMenu historyMenu = new();
                    historyMenu.Initialize();
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
        Environment.Exit(0);
    }
}