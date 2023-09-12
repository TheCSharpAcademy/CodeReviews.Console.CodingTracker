namespace CodingTracker;

class Program
{
    static void Main(string[] args)
    {   
        var database = new Database(Configuration.DatabaseFilename);
        database.CreateDatabaseIfNotPresent();
        
        var mainMenuController = new MainMenuController();
        
        var codingSessionController = new CodingSessionController(database);
        codingSessionController.SetMainMenuController(mainMenuController);
        mainMenuController.SetCodingSessionController(codingSessionController);
        
        var stopwatchController = new StopwatchController(database);
        stopwatchController.SetMainMenuController(mainMenuController);
        mainMenuController.SetStopwatchController(stopwatchController);

        var reportController = new ReportController(database);
        reportController.SetMainMenuController(mainMenuController);
        mainMenuController.SetReportController(reportController);

        mainMenuController.ShowMainMenu();
    }
}