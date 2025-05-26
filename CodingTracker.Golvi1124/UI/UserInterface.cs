using CodingTracker.Golvi1124.Data;
using CodingTracker.Golvi1124.Helpers;
using Spectre.Console;
using static CodingTracker.Golvi1124.UI.Enums;

namespace CodingTracker.Golvi1124.UI;

internal static class UserInterface
{
    internal static void MainMenu()
    {
        var isMenuRunning = true;

        while (isMenuRunning)
        {
            var usersChoice = AnsiConsole.Prompt(
                   new SelectionPrompt<MainMenuChoices>()
                    .Title("What would you like to do?")
                    .AddChoices(
                       MainMenuChoices.RunStopwatch,
                       MainMenuChoices.AddRecord,
                       MainMenuChoices.ViewRecords,
                       MainMenuChoices.UpdateRecord,
                       MainMenuChoices.DeleteRecord,
                       MainMenuChoices.AnalyseRecords,
                       MainMenuChoices.Quit)
                    .UseConverter(choice => Extras.GetEnumDisplayName(choice)) // Display the enum name
                    );

            switch (usersChoice)
            {
                case MainMenuChoices.AddRecord:
                    RecordOperations.AddRecord();
                    break;

                case MainMenuChoices.ViewRecords:
                    var dataAccess = new DataAccess();
                    var records = dataAccess.GetAllRecords();
                    RecordOperations.ViewRecords(records);
                    break;

                case MainMenuChoices.UpdateRecord:
                    RecordOperations.UpdateRecord();
                    break;

                case MainMenuChoices.DeleteRecord:
                    RecordOperations.DeleteRecord();
                    break;

                case MainMenuChoices.RunStopwatch:
                    RecordOperations.RunStopwatchSession();
                    break;

                case MainMenuChoices.AnalyseRecords:
                    RunAnalyseMenu();
                    break;

                case MainMenuChoices.Quit:
                    System.Console.WriteLine("Goodbye");
                    isMenuRunning = false;
                    break;
            }
        }
    }

    internal static void RunAnalyseMenu()
    {
        var isAnalyseMenuRunning = true;

        while (isAnalyseMenuRunning)
        {
            var usersChoice = AnsiConsole.Prompt(
                   new SelectionPrompt<AnalyseMenuChoices>()
                    .Title("What would you like to analyse?")
                    .AddChoices(
                       AnalyseMenuChoices.FilterPerPeriod,
                       AnalyseMenuChoices.ViewTotalAndAverage,
                       AnalyseMenuChoices.IsGoalReached,
                       AnalyseMenuChoices.Back)
                    .UseConverter(choice => Extras.GetEnumDisplayName(choice))
                    );

            switch (usersChoice)
            {
                case AnalyseMenuChoices.FilterPerPeriod:
                    var dataAccess = new DataAccess();
                    var records = dataAccess.GetAllRecords();
                    AnalyseOperations.FilterPerPeriod(records);
                    break;


                case AnalyseMenuChoices.ViewTotalAndAverage:
                    AnalyseOperations.ViewTotalAndAverage();
                    break;

                case AnalyseMenuChoices.IsGoalReached:
                    AnalyseOperations.IsGoalReached();
                    break;

                case AnalyseMenuChoices.Back:
                    isAnalyseMenuRunning = false;
                    break;
            }
        }
    }
}