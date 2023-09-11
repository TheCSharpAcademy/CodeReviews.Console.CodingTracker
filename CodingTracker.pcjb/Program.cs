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
        
        mainMenuController.ShowMainMenu();
    }
}