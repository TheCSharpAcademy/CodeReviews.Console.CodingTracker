using System.Configuration;

using CodingTracker.MartinL_no.DAL;
using CodingTracker.MartinL_no.Controllers;
using CodingTracker.MartinL_no.UserInterface;

var dbPath = ConfigurationManager.AppSettings.Get("DbPath");
var connString = ConfigurationManager.AppSettings.Get("ConnString");

var repo = new CodingSessionRepository(connString, dbPath);
var controller = new CodingController(repo);
var userInterface = new UserInput(controller);

userInterface.Execute();

//var s = controller.GetCodingSessionsByYears(3);
//Console.WriteLine();
