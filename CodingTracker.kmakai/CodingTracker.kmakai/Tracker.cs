using CodingTracker.kmakai.Controllers;
using CodingTracker.kmakai.Data;

namespace CodingTracker.kmakai;

public class Tracker
{
    private readonly DbContext DbContext;
    private readonly CodeSessionController CodeSessionController;
    private readonly ViewController ViewController;
    private bool IsRunning = true;

    public Tracker()
    {
        DbContext = new DbContext();
        CodeSessionController = new CodeSessionController(DbContext);
        ViewController = new ViewController();
    }

    public void Start()
    {
        while (IsRunning)
        {
            ViewController.MainMenu();
            var input = InputController.GetMainMenuInput();

            switch (input)
            {
                case 1:
                    ViewController.ViewSessions(CodeSessionController.CodeSessions);
                    break;
                case 2:
                    CodeSessionController.CreateSession();
                    break;
                case 3:
                    CodeSessionController.UpdateSession();
                    break;
                case 4:
                    CodeSessionController.DeleteSession();
                    break;
                case 0:
                    IsRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid input.");
                    break;
            }

        }
    }


}
