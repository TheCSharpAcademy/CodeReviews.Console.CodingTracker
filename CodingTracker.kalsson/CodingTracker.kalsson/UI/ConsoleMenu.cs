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

    /// <summary>
    /// Adds a coding session to the tracker.
    /// </summary>
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

        // Create a new CodingSession
        CodingSession session = new CodingSession
        {
            StartTime = startTime,
            EndTime = endTime
        };

        // Use the repository to insert the new coding session into the database
        try
        {
            _repository.InsertCodingSession(session);
            AnsiConsole.MarkupLine("[green]The coding session was added successfully![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error adding session: {ex.Message}[/]");
        }
    }

    private void ViewSessions()
    {
        Console.Clear();  // Clear the console for a clean display
        AnsiConsole.MarkupLine("[underline green]Session List[/]");  // Title for clarity

        var sessions = _repository.GetAllCodingSessions().ToList();
        if (!sessions.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No sessions found to display.[/]");
        }
        else
        {
            // Display sessions in a table
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn(new TableColumn("[u]ID[/]").Centered());
            table.AddColumn(new TableColumn("[u]Start Time[/]").Centered());
            table.AddColumn(new TableColumn("[u]End Time[/]").Centered());

            foreach (var session in sessions)
            {
                table.AddRow(session.Id.ToString(),
                    session.StartTime.ToString("yyyy-MM-dd HH:mm"),
                    session.EndTime.ToString("yyyy-MM-dd HH:mm"));
            }

            AnsiConsole.Write(table);
        }

        // Prompt to return to the main menu
        AnsiConsole.MarkupLine("[grey]Press any key to return to the main menu...[/]");
        Console.ReadKey();
    }

    private void UpdateSession()
    {
        var sessions = _repository.GetAllCodingSessions().ToList();
        if (!sessions.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No sessions available to update.[/]");
            return;
        }

        // Display sessions in a table
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn("ID");
        table.AddColumn("Start Time");
        table.AddColumn("End Time");

        foreach (var session in sessions)
        {
            table.AddRow(session.Id.ToString(), session.StartTime.ToString("yyyy-MM-dd HH:mm"), session.EndTime.ToString("yyyy-MM-dd HH:mm"));
        }

        AnsiConsole.Write(table);

        // Ask for session selection
        var selectedSession = AnsiConsole.Prompt(
            new SelectionPrompt<CodingSession>()
                .Title("Select a session to update:")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more sessions)[/]")
                .UseConverter(s => $"ID: {s.Id} - Start: {s.StartTime:yyyy-MM-dd HH:mm} - End: {s.EndTime:yyyy-MM-dd HH:mm}")
                .AddChoices(sessions));

        // Get new start time
        var newStartTimeInput = AnsiConsole.Ask<string>("Enter the new start time (yyyy-MM-dd hh:mm): ");
        DateTime newStartTime;
        while (!DateTime.TryParse(newStartTimeInput, out newStartTime))
        {
            AnsiConsole.MarkupLine("[red]Invalid date format. Please try again.[/]");
            newStartTimeInput = AnsiConsole.Ask<string>("Enter the new start time (yyyy-MM-dd hh:mm): ");
        }

        // Get new end time
        var newEndTimeInput = AnsiConsole.Ask<string>("Enter the new end time (yyyy-MM-dd hh:mm): ");
        DateTime newEndTime;
        while (!DateTime.TryParse(newEndTimeInput, out newEndTime))
        {
            AnsiConsole.MarkupLine("[red]Invalid date format. Please try again.[/]");
            newEndTimeInput = AnsiConsole.Ask<string>("Enter the new end time (yyyy-MM-dd hh:mm): ");
        }

        // Validate end time is after start time
        if (newEndTime <= newStartTime)
        {
            AnsiConsole.MarkupLine("[red]End time must be after start time.[/]");
            return;
        }

        // Update the session details
        selectedSession.StartTime = newStartTime;
        selectedSession.EndTime = newEndTime;

        // Save the updated session
        try
        {
            _repository.UpdateCodingSession(selectedSession);
            AnsiConsole.MarkupLine("[green]The coding session has been successfully updated.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error updating session: {ex.Message}[/]");
        }
    }

    private void DeleteSession()
    {
        var sessions = _repository.GetAllCodingSessions().ToList();
        if (!sessions.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No sessions available to delete.[/]");
            return;
        }

        // Display sessions in a table
        var table = new Table();
        table.Border(TableBorder.Rounded); // Adds rounded borders to the table
        table.AddColumn(new TableColumn("[u]ID[/]").Centered());
        table.AddColumn(new TableColumn("[u]Start Time[/]").Centered());
        table.AddColumn(new TableColumn("[u]End Time[/]").Centered());

        foreach (var session in sessions)
        {
            table.AddRow(session.Id.ToString(), 
                session.StartTime.ToString("yyyy-MM-dd HH:mm"), 
                session.EndTime.ToString("yyyy-MM-dd HH:mm"));
        }

        AnsiConsole.Write(table);

        // Ask for session ID to delete
        var sessionId = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter the ID of the session you wish to delete:")
                .Validate(id =>
                {
                    // Validates that the entered ID exists in the list of sessions
                    var valid = sessions.Any(s => s.Id == id);
                    return valid ? ValidationResult.Success() : ValidationResult.Error("[red]ID not found![/]");
                }));

        // Confirm deletion
        var confirmDelete = AnsiConsole.Confirm("Are you sure you want to delete this session?", false);
        if (!confirmDelete)
        {
            AnsiConsole.MarkupLine("[grey]Delete canceled.[/]");
            return;
        }

        // Delete the session
        try
        {
            _repository.DeleteCodingSession(sessionId);
            AnsiConsole.MarkupLine("[green]Session deleted successfully.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error deleting session: {ex.Message}[/]");
        }
    }
}