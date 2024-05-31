using System.Configuration;

public class App
{
    private DatabaseManager _databaseManager = new DatabaseManager(ConfigurationManager.ConnectionStrings["CodingTrackerDB"].ConnectionString);
    private UserInput _userInput = new UserInput();

    public void Run()
    {
        _databaseManager.InitializeDatabase();

        var options = _userInput.MainMenu();

        switch (options)
        {
            case MainMenuOptions.NewSession: break;
            case MainMenuOptions.ExistingSession: break;
            case MainMenuOptions.AddManualSession: break;
            case MainMenuOptions.ViewSessions: break; // Let the user filter their coding records per period (weeks, days, years) and/or order ascending or descending.
            case MainMenuOptions.Goals: break;
            case MainMenuOptions.Reports: break; // options (years, weeks, days). Breakdown by filter. 
            case MainMenuOptions.Exit: break;
        }

        Console.ReadKey();
    }
}