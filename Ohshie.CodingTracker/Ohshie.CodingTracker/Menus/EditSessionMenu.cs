namespace Ohshie.CodingTracker.Menus;

public class EditSessionMenu : MenuBase
{
    public EditSessionMenu(int id)
    {
        Id = id;
    }

    private readonly SessionsDisplay _display = new();
    private int Id { get; }

    private readonly SessionEditor _sessionEditor = new();
    
    protected override bool Menu()
    {
        Console.Clear();
        AnsiConsole.Write(new Rule("Editing session menu"));
        if (!_display.ShowSession(Id))
        {
            Errors.DoesNotExist("session with that ID is not present in database.");
            return true;
        }
        
        var userChoice = MenuBuilder();

        switch (userChoice)
        {
            case "Edit session Name(description)":
            {
                EditName();
                break;
            }
            case "Edit session Date":
            {
                break;
            }
            case "Edit session Length":
            {
                break;
            }
            case "Edit session Note":
            {
                EditNote();
                break;
            }
            case "Delete session":
            {
                DeleteSession();
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
                .PageSize(6)
                .AddChoices(new[]
                {
                    "Edit session Name(description)", 
                    "Edit session Date", 
                    "Edit session Length",
                    "Edit session Note",
                    "Delete session",
                    "Go back"
                }));
    }
    
    private void EditName()
    {
        var userInput = AskUserForNewData();
        if (Verify.GoBack(userInput)) return;

        _sessionEditor.EditSessionName(userInput!, Id);
    }
    
    private void EditDate()
    {
        
    }

    private void EditLength()
    {
        
    }

    private void EditNote()
    {
        var userInput = AskUserForNewData();
        if (Verify.GoBack(userInput)) return;
        
        _sessionEditor.EditSessionNote(userInput!,Id);
    }

    private void DeleteSession()
    {
        AnsiConsole.Write(new Markup("You are about to [bold]delete[/] this session\n"));
        if (!AnsiConsole.Confirm("Confirm?"))
        {
            return;
        }
        
        _sessionEditor.RemoveSession(Id);
    }
    
    private string? AskUserForNewData()
    {
        Console.Clear();
        Console.Write("Please type new name for chosen session: \n" +
                      "leave this field empty or type back to go back.\n");
        string? userInput = Console.ReadLine();
        return userInput;
    }
}