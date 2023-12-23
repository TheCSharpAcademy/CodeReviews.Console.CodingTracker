using Ohshie.CodingTracker.SessionsOperator;

namespace Ohshie.CodingTracker.Menus;

internal class ChooseSessionDialog
{
    private SessionsDisplay _display = new();

    public void Initialize()
    {
        bool chosenExit = false;
        while (!chosenExit)
        {
            Console.Clear();
            if (!_display.ShowSessions())
            {
                Errors.DoesNotExist(errorMessage: "consider recording session first");
                return;
            }
            
            Console.WriteLine("Enter session id to edit session stats or");
            Console.WriteLine("Press enter or type no to go back\n");

            var userInput = Console.ReadLine();
            if (Verify.GoBack(userInput)) return;
            
            if (!int.TryParse(userInput, out int id)) continue;

            EditSessionMenu sessionMenu = new(id);
            sessionMenu.Initialize();
        }
    }
}