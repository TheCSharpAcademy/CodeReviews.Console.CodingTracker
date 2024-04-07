using coding_tracker;
using System.Configuration;

bool testMode = true;

string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;
DatabaseManager databaseManager = new();
SessionController sessionController = new();
UserInput userInput = new(sessionController);
sessionController.UserInput = userInput;

databaseManager.SqlInitialize(connectionString);
userInput.Testing(testMode);
userInput.GoalMenu();
userInput.MainMenu();