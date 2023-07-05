using CodingTracker.alvaromosconi.ConsoleUI;
using CodingTracker.alvaromosconi.Controller;
using CodingTracker.alvaromosconi.Data;

var sessionStorage = new CodeSessionLocalStorage();
var controller = new CodingController(sessionStorage);
var view = new ConsoleView(controller);
view.WelcomeMessage();
view.Menu();