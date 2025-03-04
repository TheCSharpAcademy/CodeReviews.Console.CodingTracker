using CodingTracker.selnoom.Data;
using CodingTracker.selnoom.Models;
using CodingTracker.selnoom.UI;
using Spectre.Console;

namespace CodingTracker.selnoom.Helpers;

internal static class Helpers
{
    internal static void ShowSessions(List<CodingHours> sessions)
    {
        if (sessions.Count > 0)
        {
            AnsiConsole.MarkupLine("[bold]Sessions:[/]\n");
            foreach (CodingHours session in sessions)
            {
                AnsiConsole.MarkupLine($"{session.Id}\tStart Time: {session.StartTime}\tEnd Time: {session.EndTime}\tDuration: {Validation.FormatDuration(session.Duration)}");
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[bold]No sessions yet.[/]\n");
        }
    }

    internal static void UpdateSession(List<CodingHours> sessions, CodingHoursRepository repository)
    {
        int? chosenId = ChooseSessionId(sessions);
        if (chosenId == null)
        {
            return;
        }

        (string newStartTime, string newEndTime) = MainMenu.GetStartAndEndTimes();
        if (Validation.ReturnToMenu(newStartTime))
        {
            return;
        }

        repository.UpdateRecord(chosenId.Value, newStartTime, newEndTime);

        AnsiConsole.MarkupLine("[bold green]Entry was successfully updated! Press enter to continue[/]");
        AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
    }
    internal static void DeleteSession(List<CodingHours> sessions, CodingHoursRepository repository)
    {
        int? chosenId = ChooseSessionId(sessions);
        if (chosenId == null)
        {
            return;
        }

        repository.DeleteRecord(chosenId.Value);

        AnsiConsole.MarkupLine("[bold green]Entry was successfully deleted! Press enter to continue[/]");
        AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
    }

    internal static int? ChooseSessionId(List<CodingHours> sessions)
    {
        if (sessions.Count == 0)
        {
            AnsiConsole.MarkupLine("[bold]No sessions yet.[/]\n");
            AnsiConsole.MarkupLine("\nPress enter to continue");
            AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
            return null;
        }

        List<int> sessionIds = sessions.Select(x => x.Id).ToList();

        while (true)
        {
            string userInput = AnsiConsole.Ask<string>("\nPlease type the Id of the session you wish to select or 0 to return:");

            if (Validation.ReturnToMenu(userInput))
            {
                return null;
            }

            int chosenId = Validation.FormatInputToInt(userInput);
            if (Validation.CheckIfIdExists(sessionIds, chosenId))
            {
                return chosenId;
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]The selected Id does not exist. Please try again.[/]\n");
            }
        }
    }
}
