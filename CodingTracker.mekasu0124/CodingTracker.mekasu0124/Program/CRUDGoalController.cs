using CodingTracker.Models;
using CodingTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.Program;

public class CRUDGoalController
{
    private static readonly List<string?>? goalColHeaders = new() { "Id", "Name", "Start Date", "End Date", "Days To Goal", "Hours Per Day", "Achieved" };

    public static void NewGoal()
    {
        Console.Clear();
        Console.WriteLine("-------------------");
        Console.WriteLine("Creating A New Goal");
        Console.WriteLine("-------------------");

        Console.Write("\nNew Goal Name: ");
        string? input = Console.ReadLine();
        string? goalName = UserValidation.ValidateAlphaInput(input, "New Goal Name: ");

        Console.Write("\nNew Goal Start Date: ");
        input = Console.ReadLine();
        string? startDate = UserValidation.VerifyDateInput(input);

        Console.Write("\nNew Goal End Date: ");
        input = Console.ReadLine();
        string? endDate = UserValidation.VerifyDateInput(input);

        Console.WriteLine("\nHow Many Hours Per Day Do You Plan To Code (Whole Number)?");
        Console.Write("Your Selection: ");
        input = Console.ReadLine();
        int? hoursPerDay = UserValidation.ValidateNumericInput(input);

        int? daysToGoal = Helpers.DaysToGoal(startDate, endDate);

        Console.WriteLine("------------------------------------");
        Console.WriteLine($"Goal Name: {goalName}");
        Console.WriteLine($"Start Date: {startDate}");
        Console.WriteLine($"End Date: {endDate}");
        Console.WriteLine($"Coding Hours Per Day: {hoursPerDay}");
        Console.WriteLine($"Days Until Goal: {daysToGoal}");
        Console.WriteLine($"Goal Achieved: No");
        Console.WriteLine("------------------------------------");

        bool satisfied = UserValidation.ValidateYesNo("Are You Satisfied With This Information? Y/N");

        Goal? newGoal = new()
        {
            Name = goalName,
            StartDate = startDate,
            EndDate = endDate,
            DaysToGoal = daysToGoal,
            HoursPerDay = hoursPerDay,
            Achieved = "No"
        };

        if (satisfied)
        {
            GoalQueries.NewGoal(newGoal);
        }
        else
        {
            Console.WriteLine("You Entered No. Press ENTER To Go Back To Main Menu.");
            Console.ReadLine();
            Console.Clear();
            MainMenu.ShowMenu();
        }
    }

    public static void EditGoal()
    {
        Console.Clear();
        Console.WriteLine("-----------------");
        Console.WriteLine("Editing A Goal");
        Console.WriteLine("-----------------");

        List<Goal?>? allGoals = GoalQueries.GetAllGoals();
        List<List<object?>?>? tableData = new List<List<object?>?>();

        foreach (Goal? goal in allGoals)
        {
            List<object?>? item = new List<object?>() {
                goal.Id, goal.Name, goal.StartDate, goal.EndDate, goal.DaysToGoal, goal.HoursPerDay, goal.Achieved
            };

            tableData.Add(item);
        }

        TableVisualizationEngine.ShowTable(tableData, goalColHeaders, "All Coding Goals");

        Console.WriteLine("\nTo Select A Goal To Edit, Enter It's ID Number");
        Console.Write("Your Selection: ");
        string? input = Console.ReadLine();
        int? selectedId = UserValidation.ValidateIdSelection(UserValidation.ValidateNumericInput(input), null, allGoals);

        Goal? currentGoal = GoalQueries.GetSelectedGoal(selectedId);
        Goal? updatedGoal = StartGoalEdit(currentGoal);
        GoalQueries.UpdateGoal(updatedGoal);
    }

    public static Goal? StartGoalEdit(Goal? currentGoal)
    {
        Console.WriteLine("\nWhen Editing Information, If you want it left the same then press ENTER to skip.");
        Console.WriteLine($"\nCurrent Name: {currentGoal.Name}");
        Console.Write("Enter Corrected Name: ");

        string? input = Console.ReadLine();
        bool nameChanged = UserValidation.VerifyEmptyOrChanged(currentGoal.Name, input);
        string? newName = nameChanged
            ? UserValidation.ValidateAlphaInput(input, "Enter Corrected Name")
            : currentGoal.Name;

        Console.WriteLine($"\nCurrent Start Date: {currentGoal.StartDate}");
        Console.Write("Enter Corrected Start Date: ");

        input = Console.ReadLine();
        bool startDateChanged = UserValidation.VerifyEmptyOrChanged(currentGoal.StartDate, input);
        string? newStartDate = startDateChanged
            ? UserValidation.VerifyDateInput(input)
            : currentGoal.StartDate;

        Console.WriteLine($"\nCurrent End Date: {currentGoal.EndDate}");
        Console.Write("Enter Corrected End Date: ");

        input = Console.ReadLine();
        bool endDateChanged = UserValidation.VerifyEmptyOrChanged(currentGoal.EndDate, input);
        string? newEndDate = endDateChanged
            ? UserValidation.VerifyDateInput(input)
            : currentGoal.EndDate;

        Console.WriteLine($"\nCurrent Hours Per Day: {currentGoal.HoursPerDay}");
        Console.WriteLine("Enter Corrected Hours Per Day: ");
        input = Console.ReadLine();
        bool hoursChanged = UserValidation.VerifyEmptyOrChanged(currentGoal.HoursPerDay.ToString(), input);
        int? newHoursPerDay = hoursChanged
            ? UserValidation.ValidateNumericInput(input)
            : currentGoal.HoursPerDay;

        int? newDaysToGoal = Helpers.DaysToGoal(newStartDate, newEndDate);

        Goal? updatedGoal = new()
        {
            Name = newName,
            StartDate = newStartDate,
            EndDate = newEndDate,
            DaysToGoal = newDaysToGoal,
            HoursPerDay = newHoursPerDay,
            Achieved = "No"
        };

        Console.WriteLine("------------------------------------");
        Console.WriteLine($"Goal Name: {updatedGoal.Name}");
        Console.WriteLine($"Start Date: {updatedGoal.StartDate}");
        Console.WriteLine($"End Date: {updatedGoal.EndDate}");
        Console.WriteLine($"Coding Hours Per Day: {updatedGoal.HoursPerDay}");
        Console.WriteLine($"Days Until Goal: {updatedGoal.DaysToGoal}");
        Console.WriteLine($"Goal Achieved: {updatedGoal.Achieved}");
        Console.WriteLine("------------------------------------");

        Console.WriteLine();
        bool satisfied = UserValidation.ValidateYesNo("\nAre You Satisfied With This Information? Y/N");

        return satisfied ? updatedGoal : StartGoalEdit(currentGoal);
    }

    public static void DeleteGoal()
    {
        Console.Clear();
        Console.WriteLine("---------------");
        Console.WriteLine("Deleting A Goal");
        Console.WriteLine("---------------");

        List<Goal?>? allGoals = GoalQueries.GetAllGoals();
        List<List<object?>?>? tableData = new List<List<object?>?>();

        foreach (Goal? goal in allGoals)
        {
            List<object?>? item = new List<object?>()
            {
                goal.Id, goal.Name, goal.StartDate, goal.EndDate, goal.DaysToGoal, goal.HoursPerDay, goal.Achieved
            };

            tableData.Add(item);
        }

        TableVisualizationEngine.ShowTable(tableData, goalColHeaders, "All Coding Goals");

        Console.WriteLine("\nTo Select A Goal To Delete, Enter It's ID Number");
        Console.Write("Your Selction: ");
        string? input = Console.ReadLine();
        int? selectedId = UserValidation.ValidateIdSelection(UserValidation.ValidateNumericInput(input), null, allGoals);

        Goal? currentGoal = GoalQueries.GetSelectedGoal(selectedId);

        GoalQueries.DeleteGoal(currentGoal);
    }

    public static void ViewAllGoals()
    {
        Console.Clear();

        List<Goal?>? allGoals = GoalQueries.GetAllGoals();
        List<List<object?>?>? tableData = new List<List<object?>?>();

        int totalGoalDays = Helpers.GetTotalGoalDays(allGoals);
        int totalGoalHours = Helpers.GetTotalGoalHours(allGoals);
        double totalDaysUntilAllGoalsMet = Helpers.GetTotalDaysUntilAllGoalsMet(allGoals);

        foreach (Goal? goal in allGoals)
        {
            List<object?>? item = new List<object?>()
            {
                goal.Id, goal.Name, goal.StartDate, goal.EndDate, goal.DaysToGoal, goal.HoursPerDay, goal.Achieved
            };

            tableData.Add(item);
        }

        TableVisualizationEngine.ShowTable(tableData, goalColHeaders, "All Coding Goals");

        Console.WriteLine("\nTotal Goal Days: {0}", totalGoalDays);
        Console.WriteLine("Total Goal Hours: {0}", totalGoalHours);
        Console.WriteLine("Total Days Until All Goals Are Met: {0}\n", totalDaysUntilAllGoalsMet);

        MainMenu.ShowMenu();
    }
}
