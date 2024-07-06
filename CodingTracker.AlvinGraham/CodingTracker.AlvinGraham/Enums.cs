using Microsoft.OpenApi.Extensions;
using System.ComponentModel.DataAnnotations;

namespace CodingTracker;

internal static class Enums
{
	internal enum MainMenuChoices
	{
		[Display(Name = "Add Record")]
		AddRecord,

		[Display(Name = "View Records")]
		ViewRecords,

		[Display(Name = "Delete Record")]
		DeleteRecord,

		[Display(Name = "Update Record")]
		UpdateRecord,

		[Display(Name = "Start Live Tracking Session")]
		LiveTrack,

		[Display(Name = "Filter Coding Records")]
		FilterRecords,

		[Display(Name = "Reports")]
		Reports,

		[Display(Name = "Manage Goals")]
		ManageGoals,

		[Display(Name = "--------------------------")]
		Seperator,

		Quit
	}

	internal enum FilterMenuChoices
	{
		[Display(Name = "Sessions by Day")]
		DailySessions,

		[Display(Name = "Sessions by Week")]
		WeeklySessions,

		[Display(Name = "Sessions by Month")]
		MonthlySessions,

		[Display(Name = "Sessions by Year")]
		YearlySessions,

		[Display(Name = "All Sessions")]
		AllSessions,

		[Display(Name = "Return to Main Menu")]
		ReturnToMain
	}

	internal enum GoalMenuChoices
	{
		[Display(Name = "View Goal Progress")]
		GoalProgress,

		[Display(Name = "Set a Goal")]
		SetGoal,

		[Display(Name = "View Current Goals")]
		ViewGoals,

		[Display(Name = "Delete a Goal")]
		DeleteGoal,

		[Display(Name = "Update a Goal")]
		UpdateGoal,

		[Display(Name = "Return to Main Menu")]
		ReturnToMain
	}
}


internal class MenuChoiceConverter
{

	internal static string ChoiceToString(Enums.MainMenuChoices menuChoice)
	{
		try
		{
			var choiceString = EnumExtensions.GetAttributeOfType<DisplayAttribute>(menuChoice).GetName();
			return choiceString!;
		}
		catch (NullReferenceException)
		{
			return menuChoice.ToString();
		}
	}

	internal static string ChoiceToString(Enums.FilterMenuChoices menuChoice)
	{
		try
		{
			var choiceString = EnumExtensions.GetAttributeOfType<DisplayAttribute>(menuChoice).GetName();
			return choiceString!;
		}
		catch (NullReferenceException)
		{
			return menuChoice.ToString();
		}
	}

	internal static string ChoiceToString(Enums.GoalMenuChoices menuChoice)
	{
		try
		{
			var choiceString = EnumExtensions.GetAttributeOfType<DisplayAttribute>(menuChoice).GetName();
			return choiceString!;
		}
		catch (NullReferenceException)
		{
			return menuChoice.ToString();
		}
	}
}
