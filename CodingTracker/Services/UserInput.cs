using Spectre.Console;

public class UserInput
{

    public void Introduction()
    {
        AnsiConsole.Write(new FigletText("CodingTracker").Color(Color.Orange1));
    }

    public MainMenuOptions MainMenu()
    {
        Introduction();

        var optionDescriptions = new Dictionary<string, MainMenuOptions>
        {
            { "Start a new session", MainMenuOptions.NewSession },
            { "Continue an existing session", MainMenuOptions.ExistingSession },
            { "Add a session manually", MainMenuOptions.AddManualSession },
            { "View & update previous sessions", MainMenuOptions.ViewSessions },
            { "Add, update or view goals", MainMenuOptions.Goals }, 
            { "View Reports", MainMenuOptions.Reports }, 
            { "Exit the application", MainMenuOptions.Exit }
        };

        var options = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What would you like todo?")
                .PageSize(10)
                .AddChoices(optionDescriptions.Keys));

        return optionDescriptions[options];
    }
}