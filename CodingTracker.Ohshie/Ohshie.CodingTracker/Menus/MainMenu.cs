using Ohshie.CodingTracker.SessionsOperator;

namespace Ohshie.CodingTracker.Menus;

public class MainMenu
{
    public void Initialize()
    {
        bool chosenExit = false;
        while (!chosenExit)
        {
            chosenExit = Menu();
        }
        Environment.Exit(0);
    }

    private bool Menu()
    {
        Console.Clear();
        AnsiConsole.Write(new Rule("Hey! Time to put your head down and [bold]code.[/]"));
        
        var userChoise = MenuBuilder();

        switch (userChoise)
        {
            case "New Coding Tracker":
            {
                NewCodingSession session = new NewCodingSession();
                session.NewSession();
                break;
            }
            case "Show Previous Coding Sessions":
            {
                HistoryMenu historyMenu = new();
                historyMenu.Initialize();
                break;
            }
            case "Exit":
            {
                return true;
            }
        }

        return false;
    }

    private string MenuBuilder()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose menu item:")
                .PageSize(3)
                .AddChoices(new[]
                {
                    "New Coding Tracker", 
                    "Show Previous Coding Sessions", 
                    "Exit"
                }));
    }
}