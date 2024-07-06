using Spectre.Console;
using static CodingTracker.Enums;

namespace CodingTracker;

internal class Goals
{

	private List<GoalRecord> goalList = new List<GoalRecord>();
	public Goals()
	{
		var dataAccess = new GoalDataAccess();
		goalList = dataAccess.GetGoalList();
	}

	internal void goalsMenu()
	{
		var isMenuRunning = true;

		while (isMenuRunning)
		{
			Utilities.ClearScreen("Managing Goals");

			var selectionPrompt = new SelectionPrompt<GoalMenuChoices>()
				.Title("What would you like to do?")
				.AddChoices(
					GoalMenuChoices.GoalProgress,
					GoalMenuChoices.ViewGoals,
					GoalMenuChoices.SetGoal,
					GoalMenuChoices.UpdateGoal,
					GoalMenuChoices.DeleteGoal,
					GoalMenuChoices.ReturnToMain
					);
			selectionPrompt.Converter = MenuChoiceConverter.ChoiceToString;

			var userChoice = AnsiConsole.Prompt(selectionPrompt);

			switch (userChoice)
			{
				case GoalMenuChoices.GoalProgress:
					Utilities.ClearScreen("Goal Progress View");
					GoalProgress();
					break;
				case GoalMenuChoices.ViewGoals:
					Utilities.ClearScreen("Viewing Goals");
					ViewGoals();
					Console.WriteLine("\nPress any key to return to goals menu.");
					Console.ReadKey();
					break;
				case GoalMenuChoices.SetGoal:
					Utilities.ClearScreen("Setting New Goal");
					SetGoals();
					break;
				case GoalMenuChoices.UpdateGoal:
					Utilities.ClearScreen("Updating an Exitsting Goal");
					UpdateGoal();
					break;
				case GoalMenuChoices.DeleteGoal:
					Utilities.ClearScreen("Deleting a Goal");
					DeleteGoal();
					break;
				case GoalMenuChoices.ReturnToMain:
					return;
			}
		}
	}

	private void DeleteGoal()
	{
		ViewGoals();
		var id = Utilities.GetGoalNumber("Please type the id of the goal you want to Delete.");

		if (!AnsiConsole.Confirm("Are you sure?"))
			return;

		var dataAccess = new GoalDataAccess();
		var response = dataAccess.DeleteGoal(id);

		var responseMessage = response < 1
	? $"Record with id {id} doesn't exit. Press any key to return to Main Menu."
	: "Record deleted successfully. Press any key to return to Goals Menu";

		Console.WriteLine(responseMessage);
		Console.ReadKey();
	}

	private void UpdateGoal()
	{
		ViewGoals();

		var id = Utilities.GetGoalNumber("Please type the id of the goal you want to update.");

		var goal = goalList.Where(x => x.Id == id).Single();

		int goalHours = goal.TotalMinutes / 60;
		int goalMinutes = goalHours % 60;
		DateTime endDate = goal.DateEnd;

		if (AnsiConsole.Confirm("Would you like to modify the coding time goal?"))
		{
			goalHours = Utilities.GetGoalNumber("Enter hours for your coding goal: ");
			goalMinutes = Utilities.GetGoalNumber("Enter minutes for your coding goal: ");
		}

		if (AnsiConsole.Confirm("Would you like to modify the coding goal end date?"))
		{
			endDate = Utilities.GetEndDate();
		}

		goal.Id = id;
		goal.TotalMinutes = goalHours * 60 + goalMinutes;
		goal.DateEnd = endDate;

		var dataAccess = new GoalDataAccess();
		dataAccess.UpdateGoal(goal);

		Console.WriteLine("\nPress any key to return to goals menu.");
		Console.ReadKey();
	}


	private static void SetGoals()
	{
		GoalRecord record = new();

		int goalHours = Utilities.GetGoalNumber("Enter hours for your coding goal: ");
		int goalMinutes = Utilities.GetGoalNumber("Enter minutes for your coding goal: ");

		record.TotalMinutes = 60 * goalHours + goalMinutes;
		record.DateEnd = Utilities.GetEndDate();

		var dataAccess = new GoalDataAccess();
		dataAccess.InsertGoal(record);
	}

	private void ViewGoals()
	{
		var dataAccess = new GoalDataAccess();
		goalList = dataAccess.GetGoalList();

		var table = new Table();
		table.AddColumns("Id", "Coding Goal", "Goal End Date");
		foreach (var goal in goalList)
		{
			table.AddRow(goal.Id.ToString(), $"{goal.TotalMinutes / 60} Hours {goal.TotalMinutes % 60} Minutes",
				goal.DateEnd.ToString());
		}

		AnsiConsole.Write(table);
	}

	private void GoalProgress()
	{
		ViewGoals();

		var recordDataAccess = new DataAccess();
		var recordList = recordDataAccess.GetRecordList();

		var id = Utilities.GetGoalNumber("Please enter the id of the goal you want to review: ");
		var goal = goalList.Where(x => x.Id == id).Single();
		int totalCodingMinutes = 0;
		foreach (var record in recordList)
		{
			totalCodingMinutes += (int)record.Duration.TotalMinutes;
		}

		if (goal.DateEnd < DateTime.Now)
		{
			Console.WriteLine("\nThe end date for this goal has passed.");
			totalCodingMinutes = 0;
			foreach (var record in recordList)
			{
				if (record.DateEnd < goal.DateEnd)
					totalCodingMinutes += (int)record.Duration.TotalMinutes;
			}
			if (totalCodingMinutes > goal.TotalMinutes)
			{
				Console.WriteLine($"Congrats! {totalCodingMinutes / 60} Hours and {totalCodingMinutes % 60} Minutes of the goal of {goal.TotalMinutes / 60} Hours and {goal.TotalMinutes % 60} Minutes ({(float)totalCodingMinutes / goal.TotalMinutes:P2}) completed!");
			}
			else
			{
				Console.WriteLine($"Booo! {totalCodingMinutes / 60} Hours and {totalCodingMinutes % 60} Minutes of the goal of {goal.TotalMinutes / 60} Hours and {goal.TotalMinutes % 60} Minutes ({(float)totalCodingMinutes / goal.TotalMinutes:P2}) completed.");

			}
		}
		else
		{
			float percentageComplete = goal.TotalMinutes / (float)totalCodingMinutes;
			TimeSpan timeLeft = goal.DateEnd - DateTime.Now;
			int minutesLeft = goal.TotalMinutes - totalCodingMinutes;
			int goalPerDay = minutesLeft / (int)timeLeft.Days;

			Console.WriteLine($"You have completed {totalCodingMinutes / 60} Hours and {totalCodingMinutes % 60} Minutes of the goal of {goal.TotalMinutes / 60} Hours and {goal.TotalMinutes % 60} Minutes ({(float)totalCodingMinutes / goal.TotalMinutes:P2}).");
			if (minutesLeft > 0)
			{
				Console.WriteLine($"You have {timeLeft.Days} Days, {timeLeft.Hours} Hours, and {timeLeft.Minutes} Minutes left to complete {minutesLeft / 60} Hours and {minutesLeft % 60} Minutes of coding.");
				Console.WriteLine($"This will require you to complete {goalPerDay / 60} Hours and {goalPerDay % 60} Minutes per day of coding for {timeLeft.Days} Days.");
			}
			else
				Console.WriteLine($"Congrats. You have completed this goal with {timeLeft.Days} Days to spare!");
		}
		Console.WriteLine("\nPress any key to return to goals menu.");
		Console.ReadKey();
	}
}
