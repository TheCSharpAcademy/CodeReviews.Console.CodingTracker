using CodingTracker.CSharpAcademy_Learner;
using CodingTracker.CSharpAcademy_Learner.Controllers;

DatabaseManager.InitializeDatabase();
var codingController = new CodingController();
var userInterface = new UserInterface(codingController);
userInterface.MainMenu();