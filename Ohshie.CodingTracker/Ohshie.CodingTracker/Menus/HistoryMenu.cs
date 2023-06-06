using Ohshie.CodingTracker.SessionsOperator;

namespace Ohshie.CodingTracker.Menus;

internal class HistoryMenu : MenuBase
{
    private readonly SessionsDisplay _display = new();
    private readonly SessionEditor _sessionEditor = new();
    
    protected override bool Menu()
    {
        Console.Clear();

        if (!_display.ShowSessions(5))
        {
            Errors.DoesNotExist("Consider recording session first");
            return true;
        }
        
        var userChoise = MenuBuilder();

        switch (userChoise)
        {
            case "Show/Edit previous sessions":
            {
                ChooseSessionDialog chooseSessionMenu = new();
                chooseSessionMenu.Initialize();
                break;
            }
            case "Wipe all previous data":
            {
                if (ConfirmDeletion()) _sessionEditor.WipeData();
                break;
            }
            case "Go back":
            {
                return true;
            }
        }

        return false;
    }

    protected override string MenuBuilder()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose menu item:")
                .PageSize(3)
                .AddChoices(new[]
                {
                    "Show/Edit previous sessions", 
                    "Wipe all previous data", 
                    "Go back"
                }));
    }

    private bool ConfirmDeletion()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Markup("You are about to wipe [bold]ALL[/] previous data.\n"));
        
        if (!AnsiConsole.Confirm("Confirm?")) return false;

        return true;
    }
}