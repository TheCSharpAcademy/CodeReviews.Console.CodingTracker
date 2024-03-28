using CodingTracker;

string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString")!;
DatabaseManager databaseManager = new();

CodingController codingController = new();
UserInput userInput = new(codingController);
codingController.UserInput = userInput;

databaseManager.CreateTable(connectionString);
userInput.MainMenu();
