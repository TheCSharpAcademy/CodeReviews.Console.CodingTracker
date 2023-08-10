using CodingTracker.MartinL_no.DAL;
using CodingTracker.MartinL_no.Controllers;
using CodingTracker.MartinL_no.UserInterface;

var sessionRepo = new CodingSessionRepository();
var goalsRepo = new CodingGoalRepository();

var controller = new CodingController(sessionRepo, goalsRepo);
var dateValidator = new DateValidator("yyyy-MM-dd HH:mm");

var userInterface = new UserInput(controller, dateValidator);

userInterface.Execute();
