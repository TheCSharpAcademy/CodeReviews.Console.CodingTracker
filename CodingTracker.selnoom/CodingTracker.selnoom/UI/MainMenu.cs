using CodingTracker.selnoom.Data;
using CodingTracker.selnoom.Helpers;
using CodingTracker.selnoom.Models;
using Spectre.Console;

namespace CodingTracker.selnoom.UI;

internal static class MainMenu
{
    internal static void ShowMenu(CodingHoursRepository repository)
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Markup("[bold underline]Coding Hours Tracker[/]\n\n");
            AnsiConsole.WriteLine("Select an option:");
            string userInput = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(new[]
                    {
                        "1 - View all sessions",
                        "2 - Create session",
                        "3 - Edit session",
                        "4 - Delete session",
                        "5 - Start a session",
                        "0 - Exit program"
                    })
            );

            int validatedInput = Validation.ConvertMenuInputToInt(userInput);

            switch (validatedInput)
            {
                case 0:
                    AnsiConsole.Markup("[bold green]Goodbye![/]");
                    return;
                case 1:
                    ShowSessionsMenu(repository);
                    break;
                case 2:
                    CreateMenu(repository);
                    break;
                case 3:
                    UpdateMenu(repository);
                    break;
                case 4:
                    DeleteMenu(repository);
                    break;
                case 5:
                    StartStopWatchSession(repository);
                    break;
                default:
                    return;
            }
        }
    }

    private static void ShowSessionsMenu(CodingHoursRepository repository)
    {
        AnsiConsole.Clear();
        List<CodingHours> sessions = new();
        sessions = repository.GetAllRecords();


        Helpers.Helpers.ShowSessions(sessions);

        AnsiConsole.MarkupLine("\nPress enter to continue");
        AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
    }

    internal static void CreateMenu(CodingHoursRepository repository)
    {
        AnsiConsole.Clear();

        (string, string) times = GetStartAndEndTimes();

        if (Validation.ReturnToMenu(times.Item1))
        {
            return;
        }

        repository.CreateRecord(times.Item1, times.Item2);
        AnsiConsole.MarkupLine("[bold green]Entry was successfully created! Press enter to continue[/]");
        AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
    }

    private static void UpdateMenu(CodingHoursRepository repository)
    {
        AnsiConsole.Clear();
        List<CodingHours> sessions = new();
        sessions = repository.GetAllRecords();

        Helpers.Helpers.ShowSessions(sessions);

        Helpers.Helpers.UpdateSession(sessions, repository);
    }

    private static void DeleteMenu(CodingHoursRepository repository)
    {
        AnsiConsole.Clear();
        List<CodingHours> sessions = new();
        sessions = repository.GetAllRecords();

        Helpers.Helpers.ShowSessions(sessions);

        Helpers.Helpers.DeleteSession(sessions, repository);
    }

    internal static (string, string) GetStartAndEndTimes()
    {
        string startTime;
        string endTime;

        AnsiConsole.MarkupLine("[bold]Please type the starting time of your coding session (Format: yyyy-MM-dd HH:mm) or 0 to return to the menu:[/]");
        startTime = Validation.ValidateTimeInput();
        if (Validation.ReturnToMenu(startTime))
        {
            return ("0", "0");
        }

        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold]Now, type the ending time of your coding session (Format: yyyy-MM-dd HH:mm) or 0 to return to the menu:[/]");
        endTime = Validation.ValidateEndTimeInput(startTime);
        if (Validation.ReturnToMenu(endTime))
        {
            return ("0", "0");
        }

        return (startTime, endTime);
    }

    internal static void StartStopWatchSession(CodingHoursRepository repository)
    {
        AnsiConsole.Clear();
        string input = AnsiConsole.Prompt(new TextPrompt<string>("[bold]Press enter to start your coding session or 0 to return to the menu:[/]").AllowEmpty());
        if (Validation.ReturnToMenu(input))
        {
            return;
        }

        StopWatch.StartStopWatch(repository);
    }
}
