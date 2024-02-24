using CodingTracker.Dejmenek.Controllers;
using CodingTracker.Dejmenek.DataAccess;
using CodingTracker.Dejmenek.DataAccess.Repositories;
using CodingTracker.Dejmenek.Enums;
using CodingTracker.Dejmenek.Models;
using CodingTracker.Dejmenek.Services;
using Spectre.Console;

internal class Program
{
    private static void Main(string[] args)
    {
        bool exit = false;

        DataContext.InitDatabase();

        var trackTimeService = new TrackTimeService();
        var userInteractionService = new UserInteractionService();
        var codingSessionRepository = new CodingSessionRepository();
        var goalRepository = new GoalRepository();
        var goalController = new GoalController(userInteractionService, goalRepository);
        var codingSessionController = new CodingSessionController(trackTimeService, codingSessionRepository, userInteractionService);

        while (!exit)
        {
            var userOption = AnsiConsole.Prompt(new SelectionPrompt<MenuOptions>().Title("What would you like to do?").AddChoices(
                MenuOptions.NewCodingSession,
                MenuOptions.AddCodingSession,
                MenuOptions.DeleteCodingSession,
                MenuOptions.ListCodingSessions,
                MenuOptions.GenerateMonthlyCodingSessionsReport,
                MenuOptions.GenerateYearlyCodingSessionsReport,
                MenuOptions.SetGoal,
                MenuOptions.DeleteGoal,
                MenuOptions.UpdateGoal,
                MenuOptions.ListGoals,
                MenuOptions.ShowGoalProgress,
                MenuOptions.Exit
            ));

            switch (userOption)
            {
                case MenuOptions.NewCodingSession:
                    bool shouldStop = false;
                    Thread timerThread = new Thread(() => codingSessionController.StartSession(ref shouldStop));
                    timerThread.Start();

                    AnsiConsole.MarkupLine("Press any key to stop the timer.");
                    Console.ReadKey(true);

                    shouldStop = true;

                    timerThread.Join();

                    codingSessionController.EndSession();
                    break;

                case MenuOptions.AddCodingSession:
                    codingSessionController.AddCodingSession();
                    break;

                case MenuOptions.DeleteCodingSession:
                    codingSessionController.DeleteCodingSession();
                    break;

                case MenuOptions.ListCodingSessions:
                    List<CodingSession> sessions = codingSessionController.GetAllCodingSessions();

                    var sessionsTable = new Table();

                    sessionsTable.AddColumn("Id");
                    sessionsTable.AddColumn("StartDateTime");
                    sessionsTable.AddColumn("EndDateTime");
                    sessionsTable.AddColumn("Duration");

                    foreach (var session in sessions)
                    {
                        sessionsTable.AddRow(session.Id.ToString(), session.StartDateTime, session.EndDateTime, session.Duration.ToString());
                    }

                    AnsiConsole.Write(sessionsTable);
                    ClearConsole();
                    break;

                case MenuOptions.GenerateMonthlyCodingSessionsReport:
                    var monthlyReport = codingSessionController.GetMonthlyCodingSessionReport();

                    var monthlyReportTable = new Table();

                    monthlyReportTable.AddColumn("Year");
                    monthlyReportTable.AddColumn("Month");
                    monthlyReportTable.AddColumn("Sum of coding sessions duration in minutes");

                    foreach (var (year, month, durationSum) in monthlyReport)
                    {
                        monthlyReportTable.AddRow(year, month, durationSum.ToString());
                    }

                    AnsiConsole.Write(monthlyReportTable);
                    ClearConsole();
                    break;

                case MenuOptions.GenerateYearlyCodingSessionsReport:
                    var yearlyReport = codingSessionController.GetYearlyCodingSessionReport();

                    var yearlyReportTable = new Table();

                    yearlyReportTable.AddColumn("Year");
                    yearlyReportTable.AddColumn("Sum of coding sessions duration in minutes");

                    foreach (var (year, durationSum) in yearlyReport)
                    {
                        yearlyReportTable.AddRow(year, durationSum.ToString());
                    }

                    AnsiConsole.Write(yearlyReportTable);
                    ClearConsole();
                    break;

                case MenuOptions.SetGoal:
                    goalController.AddGoal();
                    break;

                case MenuOptions.DeleteGoal:
                    goalController.DeleteGoal();
                    break;

                case MenuOptions.UpdateGoal:
                    goalController.UpdateGoal();
                    break;

                case MenuOptions.ListGoals:
                    List<Goal> goals = goalController.GetAllGoals();

                    var goalsTable = new Table();

                    goalsTable.AddColumn("Id");
                    goalsTable.AddColumn("StartDate");
                    goalsTable.AddColumn("EndDate");
                    goalsTable.AddColumn("TargetDuration");

                    foreach (var goal in goals)
                    {
                        goalsTable.AddRow(goal.Id.ToString(), goal.StartDate, goal.EndDate, goal.TargetDuration.ToString());
                    }

                    AnsiConsole.Write(goalsTable);
                    ClearConsole();
                    break;

                case MenuOptions.ShowGoalProgress:
                    var goalProgressInformations = goalController.GetGoalProgress();
                    AnsiConsole.MarkupLine($"You've coded for {goalProgressInformations.ElementAt(0).durationSum} minutes towards your goal of {goalProgressInformations.ElementAt(0).targetDuration} minutes");
                    break;

                case MenuOptions.Exit:
                    exit = true;
                    break;
            }
        }
    }

    private static void ClearConsole()
    {
        Thread.Sleep(4000);
        Console.Clear();
    }
}