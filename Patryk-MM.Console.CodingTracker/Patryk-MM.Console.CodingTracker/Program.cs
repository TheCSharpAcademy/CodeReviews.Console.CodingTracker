using Patryk_MM.Console.CodingTracker.Commands;
using Patryk_MM.Console.CodingTracker.Config;
using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Queries;
using Patryk_MM.Console.CodingTracker.Services;
using Patryk_MM.Console.CodingTracker.Utilities;
using Spectre.Console;

Database.InitializeDatabase();

var trackerService = new TrackerService();
var listSessionsHandler = new ListSessionsHandler(trackerService);
var createSessionHandler = new CreateSessionHandler(trackerService);


listSessionsHandler.Handle();
//createSessionHandler.Handle(session);
DateTime startDate, endDate;

do {
    startDate = UserInput.GetDate();
    endDate = UserInput.GetDate();

    if (!Validation.ValidateDateOrder(startDate, endDate)) {
        AnsiConsole.MarkupLine("[red]Error: The end date must be after the start date. Please try again.[/]");
    }
} while (!Validation.ValidateDateOrder(startDate, endDate));

CodingSession session = new CodingSession() {
    StartDate = startDate,
    EndDate = endDate,
};

createSessionHandler.Handle(session);