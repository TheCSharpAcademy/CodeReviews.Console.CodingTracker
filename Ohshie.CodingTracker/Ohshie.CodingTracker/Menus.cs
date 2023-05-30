public class Menus
{
    public void Main()
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
                    CodingSession session = new CodingSession();
                    session.NewSession();
                    break;
                }
                case ConsoleKey.D2:
                {
                    Console.Clear();
                    PreviosSessions();
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

    public void PreviosSessions()
    {
        Console.WriteLine("--------Hey! Time to put your head down and code.--------\n");
        
        bool chosenExit = false;
        while (!chosenExit)
        {
            Console.WriteLine("Choose menu item by pressing corresponding #:\n");
                  
            Console.WriteLine("1. Show previous coding sessions\n" +
                              "2. Wipe previous data\n" +
                              "3. Exit");
                  
            var keyPressed =Console.ReadKey(true).Key;
            
            switch (keyPressed)
            {
                case ConsoleKey.D1:
                {
                    SessionsDisplay display = new();
                    display.ShowEntries();
                    break;
                }
                case ConsoleKey.D2:
                {
                    break;
                }
                case ConsoleKey.D3:
                {
                    chosenExit = true;
                    return;
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
}