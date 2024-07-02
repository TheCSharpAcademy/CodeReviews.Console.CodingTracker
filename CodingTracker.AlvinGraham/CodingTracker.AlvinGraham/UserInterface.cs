using Spectre.Console;

namespace CodingTracker;

internal static class UserInterface
{
	internal static void MainMenu()
	{
		var isMenuRunning = true;

		while (isMenuRunning)
		{
			var userChoice = AnsiConsole.Prompt(
				new SelectionPrompt<Enums.MainMenuChoices>()
				.Title("What would you like to do?")
				.AddChoices(
					Enums.MainMenuChoices.AddRecord,
					Enums.MainMenuChoices.ViewRecords,
					Enums.MainMenuChoices.UpdateRecord,
					Enums.MainMenuChoices.DeleteRecord,
					Enums.MainMenuChoices.Quit)
				);

			switch (userChoice)
			{
				case Enums.MainMenuChoices.AddRecord:
					AddRecord();
					break;
				case Enums.MainMenuChoices.ViewRecords:
					ViewRecords();
					break;
				case Enums.MainMenuChoices.UpdateRecord:
					UpdateRecord();
					break;
				case Enums.MainMenuChoices.DeleteRecord:
					DeleteRecord();
					break;
				case Enums.MainMenuChoices.Quit:
					Console.WriteLine("Goodbye");
					isMenuRunning = false;
					break;
			}
		}

	}

	private static void DeleteRecord()
	{

	}

	private static void UpdateRecord()
	{

	}

	private static void ViewRecords()
	{

	}

	private static void AddRecord()
	{

	}
}
