using System.Configuration;

using CodingTracker.MartinL_no.DAL;
using CodingTracker.MartinL_no.Controllers;
using CodingTracker.MartinL_no.UserInterface;

var dbPath = ConfigurationManager.AppSettings.Get("DbPath");
var connString = ConfigurationManager.AppSettings.Get("ConnString");

var sessionRepo = new CodingSessionRepository(connString, dbPath);
var goalsRepo = new CodingGoalRepository(connString, dbPath);

var controller = new CodingController(sessionRepo, goalsRepo);
var userInterface = new UserInput(controller);

userInterface.Execute();
