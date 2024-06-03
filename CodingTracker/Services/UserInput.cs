using Spectre.Console;
using static System.Net.Mime.MediaTypeNames;

public class UserInput
{

    public void Header()
    {
        Console.Clear();
        AnsiConsole.Write(new FigletText("CodingTracker").Color(Color.Orange1));
    }

    public MainMenuOptions MainMenu()
    {
        Header();

        var optionDescriptions = new Dictionary<string, MainMenuOptions>
        {
            { "Start a new session", MainMenuOptions.NewSession },
            { "Add a session manually", MainMenuOptions.AddManualSession },
            { "View & update previous sessions", MainMenuOptions.ViewSessions },
            { "Add, update or view goals", MainMenuOptions.Goals }, 
            { "View Reports", MainMenuOptions.Reports }, 
            { "Exit the application", MainMenuOptions.Exit }
        };

        var options = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .PageSize(10)
                .AddChoices(optionDescriptions.Keys));

        return optionDescriptions[options];
    }

    public NewSessionOptions NewSessionMenu(TimeSpan elapsedTime, bool isRunning)
    {
        Header();

        var optionDescriptions = new Dictionary<string, NewSessionOptions>
        {
            { "Start", NewSessionOptions.Start },
            { "Reset", NewSessionOptions.Reset },
            { "Get Updated Time", NewSessionOptions.Update },
            { "Save & Exit to main menu", NewSessionOptions.Exit },
        };

        var panel = new Panel($"Coding Time: {elapsedTime}\nRunning: {isRunning}")
        {
            Header = new PanelHeader("Session Tracker"),
            Border = BoxBorder.Square
        };
        AnsiConsole.Write(panel);


        var options = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .PageSize(10)
                .AddChoices(optionDescriptions.Keys));

        return optionDescriptions[options];
    }
}