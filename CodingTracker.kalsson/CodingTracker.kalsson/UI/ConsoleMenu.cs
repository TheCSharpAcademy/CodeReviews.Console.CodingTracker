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

    /// <summary>
    /// Displays the main menu of the Coding Session Tracker application and allows the user to choose options.
    /// </summary>
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
    /// Adds a new coding session.
    /// </summary>
    private void AddSession()
    {
        var startTime = UserInput.GetDateTime("Enter the start time");
        if (startTime == null)
        {
            AnsiConsole.MarkupLine("[grey]Add session canceled.[/]");
            return;
        }

        var endTime = UserInput.GetDateTime("Enter the end time");
        if (endTime == null || !Validation.ValidateDateTimeRange(startTime.Value, endTime.Value))
        {
            AnsiConsole.MarkupLine("[red]End time must be after start time or operation canceled.[/]");
            return;
        }

        var session = new CodingSession { StartTime = startTime.Value, EndTime = endTime.Value };
        _repository.InsertCodingSession(session);
        AnsiConsole.MarkupLine("[green]Session added successfully![/]");
    }

    /// <summary>
    /// Displays a list of coding sessions.
    /// </summary>
    private void ViewSessions()
    {
        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[underline green]Session List[/]");

            var sessions = _repository.GetAllCodingSessions().ToList();
            if (!sessions.Any())
            {
                AnsiConsole.MarkupLine("[yellow]No sessions found to display.[/]");
                break; // Exit if there are no sessions to display
            }

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

            // Provide options after displaying the table
            AnsiConsole.MarkupLine(
                "[grey]Press [blue]'R'[/] to refresh or any other key to return to the main menu...[/]");
            var key = Console.ReadKey(true).Key; // Read the key without displaying it
            if (key != ConsoleKey.R)
            {
                break; // Break the loop if 'R' is not pressed, returning to main menu
            }
        }
    }

    /// <summary>
    /// Updates a coding session with new start and end times.
    /// </summary>
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
            table.AddRow(session.Id.ToString(), session.StartTime.ToString("yyyy-MM-dd HH:mm"),
                session.EndTime.ToString("yyyy-MM-dd HH:mm"));
        }

        AnsiConsole.Write(table);

        // Ask for session selection using UserInput.GetInput
        var sessionIdString = UserInput.GetInput("Enter the ID of the session you wish to update");
        if (sessionIdString == null)
        {
            AnsiConsole.MarkupLine("[grey]Update canceled.[/]");
            return;
        }

        if (!int.TryParse(sessionIdString, out int sessionId) || !sessions.Any(s => s.Id == sessionId))
        {
            AnsiConsole.MarkupLine("[red]ID not found or invalid input![/]");
            return;
        }

        var selectedSession = sessions.FirstOrDefault(s => s.Id == sessionId);

        // Get new start time using UserInput class
        var newStartTime = UserInput.GetDateTime("Enter the new start time");
        if (newStartTime == null)
        {
            AnsiConsole.MarkupLine("[grey]Update canceled.[/]");
            return;
        }

        // Get new end time using UserInput class
        var newEndTime = UserInput.GetDateTime("Enter the new end time");
        if (newEndTime == null || !Validation.ValidateDateTimeRange(newStartTime.Value, newEndTime.Value))
        {
            AnsiConsole.MarkupLine("[red]Invalid input or end time must be after start time. Update canceled.[/]");
            return;
        }

        // Update the session details
        selectedSession.StartTime = newStartTime.Value;
        selectedSession.EndTime = newEndTime.Value;

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

    /// <summary>
    /// Deletes a coding session from the repository.
    /// </summary>
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

        // Ask for session ID to delete using UserInput class
        var sessionIdString =
            UserInput.GetInput("Enter the ID of the session you wish to delete or type 'cancel' to return:");
        if (sessionIdString == null)
        {
            AnsiConsole.MarkupLine("[grey]Delete canceled.[/]");
            return;
        }

        // Attempt to parse the input as an integer
        if (!int.TryParse(sessionIdString, out int sessionId) || !sessions.Any(s => s.Id == sessionId))
        {
            AnsiConsole.MarkupLine("[red]ID not found or invalid input![/]");
            return;
        }

        // Confirm deletion using UserInput class
        if (!UserInput.ConfirmAction("Are you sure you want to delete this session?"))
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