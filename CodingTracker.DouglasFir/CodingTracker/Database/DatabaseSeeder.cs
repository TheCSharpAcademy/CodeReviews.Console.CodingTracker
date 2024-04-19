using CodingTracker.DAO;
using CodingTracker.Models;
using CodingTracker.Services;
using Spectre.Console;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System;

namespace CodingTracker.Database;

public class DatabaseSeeder
{
    private static Random _random = new Random();
    private InputHandler _inputHandler;
    private CodingSessionDao _codingSessionDAO;
    private CodingGoalDao _codingGoalDAO;

    public DatabaseSeeder(CodingSessionDao codingSessionDAO, CodingGoalDao codingGoalDAO, InputHandler inputHandler)
    {
        int seed = (int)DateTime.Now.Ticks;
        _random = new Random(seed);
        _inputHandler = inputHandler;
        _codingSessionDAO = codingSessionDAO;
        _codingGoalDAO = codingGoalDAO;
    }

    public void SeedSessions(int numOfSessions)
    {
        // number of seeds is in App.config

        int lowerBoundarySessionDuration = 60; // 60 minutes loweer boundary for duration
        int upperBoundarySessionDuration = 240; // 4 hour upper boundary for duration
        int lowerBoundarySeedDate = Utilities.GetDaysMultiplier(TimePeriod.Years) * 3; // 3 year lower seed boundary

        for (int i = 0; i < numOfSessions; i++)
        {
            DateTime endTime = DateTime.Today.AddDays(-_random.Next(1, lowerBoundarySeedDate));
            DateTime startTime = endTime.AddMinutes(-_random.Next(lowerBoundarySessionDuration, upperBoundarySessionDuration));

            CodingSessionModel newSession = new CodingSessionModel(startTime, endTime);

            _codingSessionDAO.InsertSessionAndUpdateGoals(newSession);
        }
    }

    public void SeedGoals(int numOfGoals)
    {
        DateTime currentDateTime = DateTime.UtcNow;
        TimeSpan lowerBoundaryGoalDuration = new TimeSpan(0, 30, 0); // 30 minutes lower boundary for duration
        TimeSpan upperBoundaryGoalDuration = new TimeSpan(100, 0, 0); // 100 hour upper boundary for duration

        int daysBack = _random.Next(0, 30);
        DateTime randomDatePast30Days = currentDateTime.AddDays(-daysBack);

        for (int i = 0; i < numOfGoals; i++)
        {
            TimeSpan duration = new TimeSpan(0, _random.Next(lowerBoundaryGoalDuration.Minutes, upperBoundaryGoalDuration.Hours * 60), 0);

            CodingGoalModel newGoal = new CodingGoalModel(duration, $"Seeded Goal {i}");
            newGoal.DateCreated = randomDatePast30Days.ToString(ConfigSettings.DateFormatLong);
            _codingGoalDAO.InsertNewGoal(newGoal);
        }

        // Create a master goal with a duration of 10,000 hours
        TimeSpan masterDuration = new TimeSpan(10000, 0, 0);
        CodingGoalModel masterGoal = new CodingGoalModel(masterDuration, $"10,000 Hours Goal");
        masterGoal.DateCreated = currentDateTime.AddDays(-365).ToString(ConfigSettings.DateFormatLong);
        _codingGoalDAO.InsertNewGoal(masterGoal);
    }

    public void SeedDatabase()
    {
        AnsiConsole.WriteLine("Starting database seeding...");

        try
        {
            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .Start("Processing...", ctx =>
                {
                    SeedGoals(ConfigSettings.NumberOfCodingGoalsToSeed);
                    SeedSessions(ConfigSettings.NumberOfCodingSessionsToSeed);
                });


            Utilities.DisplaySuccessMessage("Database seeded successfully!");
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error seeding database", ex.Message);
        }
        finally
        {
            _inputHandler.PauseForContinueInput();
        }
    }
}
