using CodingTracker;

var database = new DatabaseManager();
database.CreateTable();
database.SeedDatabase();

var userInput = new UserInput();
userInput.MainMenu();