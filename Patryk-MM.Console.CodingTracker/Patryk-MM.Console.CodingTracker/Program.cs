using Patryk_MM.Console.CodingTracker.Commands.Goal;
using Patryk_MM.Console.CodingTracker.Commands.Session;
using Patryk_MM.Console.CodingTracker.Config;
using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Queries.Goal;
using Patryk_MM.Console.CodingTracker.Queries.Session;
using Patryk_MM.Console.CodingTracker.Services;
using Patryk_MM.Console.CodingTracker.Utilities;
using Spectre.Console;

AnsiConsole.Write(
new FigletText("Coding Tracker")
.Centered()
.Color(Color.Green));

Database.InitializeDatabase();





var trackerService = new TrackerService();
var getSessionFromListHandler = new GetSessionFromListHandler(trackerService);
var getSessionsHandler = new GetSessionsHandler(trackerService);
var getGoalHandler = new GetGoalHandler(trackerService);

//CodingGoal createGoal = new CodingGoal() {
//    YearAndMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
//    Hours = 40,
//};
//trackerService.CreateGoal(createGoal);


//var goal = getGoalHandler.Handle();


while (true) {
    


    string choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("Please choose an option:")
        .AddChoices(["View coding sessions", "Add a coding session manually", "Track a session using a stopwatch",
            "Update existing session", "Delete a session", "Check your coding goal", "Exit the app"]));

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
        case "Check your coding goal":
            CodingGoal? goal = getGoalHandler.Handle();
            if(goal is null) {
                if (UserInput.ConfirmAction("No goal set for this month. Would you like to set one right now?")) {
                    var createGoalHandler = new CreateGoalHandler(trackerService);
                    createGoalHandler.Handle();
                    goal = getGoalHandler.Handle();
                } else break;
            } else {
                // If a goal is already set for this month, prompt the user to update it
                if (UserInput.ConfirmAction($"A goal for this month is [cyan]{goal.Hours}[/] hours. Would you like to update it?")) {
                    // Create a new instance of UpdateGoalHandler and invoke its Handle method
                    var updateGoalHandler = new UpdateGoalHandler(trackerService);
                    updateGoalHandler.Handle(goal); // Pass the existing goal to update
                                                    // Retrieve the updated goal
                    goal = getGoalHandler.Handle();
                }
            }



            var sessions = getSessionsHandler.Handle();
            sessions = sessions.Where(s => s.StartDate.Month == DateTime.Now.Month).ToList();

            var sessionTime = sessions.Aggregate(TimeSpan.Zero, (total, session) => total + session.Duration);
            double progress = sessionTime.TotalSeconds / goal.HourGoal;
            AnsiConsole.WriteLine($"Your goal for this month is {goal.Hours} hours.");
            AnsiConsole.WriteLine($"You've coded for {(int)sessionTime.TotalHours} hours and {(int)sessionTime.TotalMinutes % 60} minutes this month.");
            AnsiConsole.WriteLine($"You are {progress:P2} into your goal.");
            break;
        case "Exit the app":
            AnsiConsole.Write("Thank you for using Coding Tracker!\n");
            return;
    }
}