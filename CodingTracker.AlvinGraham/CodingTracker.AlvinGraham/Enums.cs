using Microsoft.OpenApi.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CodingTracker;
[TypeConverter(typeof(MenuChoiceConverter))]
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


}

internal class MenuChoiceConverter : TypeConverter
{

	//selectionPrompt.Converter = <MainMenuChoices, EnumExtensions.GetAttributeOfType<DisplayAttribute>(MainMenuChoices.LiveTrack).GetName()>;
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
}
