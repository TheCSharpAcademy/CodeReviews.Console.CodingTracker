using CodingTracker.kalsson.Data;
using CodingTracker.kalsson.Models;
using Spectre.Console;

namespace CodingTracker.kalsson.UI;

public class ConsoleMenu
{
    private readonly CodingSessionRepository _repository;

    public ConsoleMenu(CodingSessionRepository repository)
    {
        _repository = repository;
    }

    public void ShowMenu()
    {
        bool keepRunning = true;
        while (keepRunning)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[underline green]Coding Session Tracker[/]");
            AnsiConsole.WriteLine();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Add Session", "View Sessions", "Update Session", "Delete Session", "Exit"
                    }));

            switch (choice)
            {
                case "Add Session":
                    AddSession();
                    break;

                case "View Sessions":
                    ViewSessions();
                    break;

                case "Update Session":
                    UpdateSession();
                    break;

                case "Delete Session":
                    DeleteSession();
                    break;

                case "Exit":
                    keepRunning = false;
                    break;
            }
        }
    }

    private void AddSession()
    {

        var startTimeInput = AnsiConsole.Ask<string>("Enter the start time (yyyy-mm-dd hh:mm): ");
        DateTime startTime;
        while (!DateTime.TryParse(startTimeInput, out startTime))
        {
            AnsiConsole.MarkupLine("[red]Invalid date format. Please try again.[/]");
            startTimeInput = AnsiConsole.Ask<string>("Enter the start time (yyyy-mm-dd hh:mm): ");
        }

        var endTimeInput = AnsiConsole.Ask<string>("Enter the end time (yyyy-mm-dd hh:mm): ");
        DateTime endTime;
        while (!DateTime.TryParse(endTimeInput, out endTime))
        {
            AnsiConsole.MarkupLine("[red]Invalid date format. Please try again.[/]");
            endTimeInput = AnsiConsole.Ask<string>("Enter the end time (yyyy-mm-dd hh:mm): ");
        }

        if (endTime <= startTime)
        {
            AnsiConsole.MarkupLine("[red]End time must be after start time.[/]");
            return;
        }

        // TODO: FORTSÄTT HÄR
    }

    private void ViewSessions()
    {
        throw new NotImplementedException();
    }

    private void UpdateSession()
    {
        throw new NotImplementedException();
    }

    private void DeleteSession()
    {
        throw new NotImplementedException();
    }
}