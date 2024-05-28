using Patryk_MM.Console.CodingTracker.Commands;
using Patryk_MM.Console.CodingTracker.Config;
using Patryk_MM.Console.CodingTracker.Queries;
using Patryk_MM.Console.CodingTracker.Services;
using SQLitePCL;

Database.InitializeDatabase();

var trackerService = new TrackerService();
var listSessionsHandler = new GetSessionsHandler(trackerService);
var createSessionHandler = new CreateSessionHandler(trackerService);
var updateSessionHandler = new UpdateSessionHandler(trackerService);
var getSessionFromListHandler = new GetSessionFromListHandler(trackerService);
var getSessionsHandler = new GetSessionsHandler(trackerService);

createSessionHandler.Handle(getSessionsHandler.Handle());

//var sessionToUpdate = getSessionFromListHandler.Handle();
//updateSessionHandler.Handle(sessionToUpdate);