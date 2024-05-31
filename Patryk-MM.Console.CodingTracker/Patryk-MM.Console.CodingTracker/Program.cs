using Patryk_MM.Console.CodingTracker.Commands.Session;
using Patryk_MM.Console.CodingTracker.Config;
using Patryk_MM.Console.CodingTracker.Queries.Goal;
using Patryk_MM.Console.CodingTracker.Queries.Session;
using Patryk_MM.Console.CodingTracker.Services;
using Patryk_MM.Console.CodingTracker.Utilities;
using Spectre.Console;

Database.InitializeDatabase();
DataVisualization.PrintLogo();


var trackerService = new TrackerService();
var getSessionFromListHandler = new GetSessionFromListHandler(trackerService);
var getSessionsHandler = new GetSessionsHandler(trackerService);


while (true) {
    string choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("\nPlease choose an option:")
        .AddChoices(["View coding sessions", "Add a coding session manually", "Track a session using a stopwatch",
            "Update existing session", "Delete a session", "Generate a report", "Check your coding goal", "Exit the app"]));

    Console.Clear();

    AnsiConsole.Write(
    new FigletText("Coding Tracker")
    .Centered()
    .Color(Color.Green));

    switch (choice) {
        case "View coding sessions":
            DataVisualization.PrintSessions(getSessionsHandler.Handle());
            break;

        case "Add a coding session manually":
            AnsiConsole.MarkupLine("[bold green]MANUAL SESSION CREATION[/]");
            var createSessionHandler = new CreateSessionHandler(trackerService);
            createSessionHandler.Handle(getSessionsHandler.Handle());
            break;

        case "Track a session using a stopwatch":
            var stopwatchSessionHandler = new StopwatchSessionHandler(trackerService);
            stopwatchSessionHandler.Handle();
            break;

        case "Update existing session":
            AnsiConsole.MarkupLine("[bold green]SESSION UPDATE[/]");
            var sessionToUpdate = getSessionFromListHandler.Handle();
            if (sessionToUpdate is null) {
                AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
                break;
            }
            var allSessions = getSessionsHandler.Handle();
            var updateSessionHandler = new UpdateSessionHandler(trackerService);

            updateSessionHandler.Handle(sessionToUpdate, allSessions);
            break;
        case "Delete a session":
            AnsiConsole.MarkupLine("[bold green]SESSION DELETE[/]");
            var sessionToDelete = getSessionFromListHandler.Handle();
            if (sessionToDelete is null) {
                AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
                break;
            }
            var deleteSessionHandler = new DeleteSessionHandler(trackerService);

            deleteSessionHandler.Handle(sessionToDelete);
            break;

        case "Generate a report":
            var generateReportHandler = new GenerateReportHandler(trackerService);
            generateReportHandler.Handle();

            break;
        case "Check your coding goal":
            var checkGoalHandler = new CheckGoalHandler(trackerService);
            checkGoalHandler.Handle();
            break;
        case "Exit the app":
            AnsiConsole.Write("Thank you for using Coding Tracker!\n");
            return;
    }
}