namespace Ohshie.CodingTracker.Menus;

internal class ChooseSessionMenu
{
    private SessionsDisplay _display = new();
    private SessionEditor _sessionEditor = new();
    
    public void Initialize()
    {
        Console.Clear();
        bool chosenExit = false;
        while (!chosenExit)
        {
            if(!_display.ShowSessions()) return;
            
            Console.WriteLine("Enter session id to edit session stats or\n");
            Console.WriteLine("Press enter or type no to go back\n");

            var userInput = Console.ReadLine();
            if (Verify.GoBack(userInput)) return;

            bool correctInput = int.TryParse(userInput, out int id);
            if (correctInput)
            {
                _display.ShowSession(id);
                Console.ReadLine();
            }
            
            return;
        }
    }
}