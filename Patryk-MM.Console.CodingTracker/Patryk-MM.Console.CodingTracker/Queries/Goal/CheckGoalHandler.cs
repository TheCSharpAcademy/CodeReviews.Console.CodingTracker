using Patryk_MM.Console.CodingTracker.Commands.Goal;
using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Queries.Session;
using Patryk_MM.Console.CodingTracker.Services;
using Patryk_MM.Console.CodingTracker.Utilities;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patryk_MM.Console.CodingTracker.Queries.Goal {
    public class CheckGoalHandler {
        private readonly TrackerService _trackerService;

        public CheckGoalHandler(TrackerService trackerService)
        {
            _trackerService = trackerService;
        }

        public void Handle() {
            var getGoalHandler = new GetGoalHandler(_trackerService);
            CodingGoal? goal = getGoalHandler.Handle();
            if (goal is null) {
                if (UserInput.ConfirmAction("No goal set for this month. Would you like to set one right now?")) {
                    var createGoalHandler = new CreateGoalHandler(_trackerService);
                    createGoalHandler.Handle();
                    goal = getGoalHandler.Handle();
                } else return;
            } else {
                // If a goal is already set for this month, prompt the user to update it
                if (UserInput.ConfirmAction($"A goal for this month is [cyan]{goal.Hours}[/] hours. Would you like to update it?")) {
                    // Create a new instance of UpdateGoalHandler and invoke its Handle method
                    var updateGoalHandler = new UpdateGoalHandler(_trackerService);
                    updateGoalHandler.Handle(goal); // Pass the existing goal to update
                                                    // Retrieve the updated goal
                    goal = getGoalHandler.Handle();
                }
            }


            var getSessionsHandler = new GetSessionsHandler(_trackerService);
            var sessions = getSessionsHandler.Handle();
            sessions = sessions.Where(s => s.StartDate.Month == DateTime.Now.Month).ToList();

            var sessionTime = sessions.Aggregate(TimeSpan.Zero, (total, session) => total + session.Duration);
            double progress = sessionTime.TotalSeconds / goal.HourGoal;
            System.Console.Clear();
            DataVisualization.PrintLogo();
            AnsiConsole.MarkupLine($"Your goal for this month is [cyan]{goal.Hours}[/] hours.");
            AnsiConsole.MarkupLine($"You've coded for [cyan]{(int)sessionTime.TotalHours}[/] hours and [cyan]{(int)sessionTime.TotalMinutes % 60}[/] minutes this month.");
            if (progress >= 1) AnsiConsole.MarkupLine("[cyan]Congratulations! You have satisfied your monthly goal![/]");
            else AnsiConsole.MarkupLine($"You are [cyan]{progress:P2}[/] into your goal.");
        }
    }
}
