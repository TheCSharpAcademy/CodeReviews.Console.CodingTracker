using Spectre.Console;
using CodingTracker.Models;
using CodingTracker.Data;

namespace CodingTracker.Controllers;

internal class CodingController
{
  private readonly Database _database = new Database();

  public void ViewSessions()
  {
    var table = new Table();
    table.Border(TableBorder.Rounded);
    table.AddColumn("[yellow]ID[/]");
    table.AddColumn("[yellow]StartTime[/]");
    table.AddColumn("[yellow]EndTime[/]");
    table.AddColumn("[yellow]Duration[/]");
    table.AddColumn("[yellow]Description[/]");

    var sessions = _database.GetAll();
    foreach (var session in sessions)
    {
      table.AddRow(
          session.Id.ToString(),
          session.StartTime.ToString("g"),
          session.EndTime.ToString("g"),
          $"{(session.EndTime - session.StartTime).TotalMinutes} mins",
          session.Description ?? ""
          );
    }

    AnsiConsole.Write(table);
    AnsiConsole.MarkupLine("Press Any Key to Continue.");
    Console.ReadKey();
  }
  public void AddSession()
  {
    var startTime = AnsiConsole.Ask<DateTime>("Enter the [green]start time[/] of the coding session (e.g., 2024-01-01 14:00):");
    var endTime = AnsiConsole.Ask<DateTime>("Enter the [green]end time[/] of the coding session (e.g., 2024-01-01 16:00):");
    var description = AnsiConsole.Ask<string>("Enter a [green]description[/] for the coding session (optional):");

    var newSession = new CodingSession
    {
      StartTime = startTime,
      EndTime = endTime,
      Description = description
    };

    _database.Insert(newSession);
    AnsiConsole.MarkupLine("[green]Coding session added successfully![/]");
    AnsiConsole.MarkupLine("Press Any Key to Continue.");
    Console.ReadKey();
  }
  public void DeleteSession()
  {
    var id = AnsiConsole.Ask<int>("Enter the [green]ID[/] of the session to delete:");
    _database.Delete(id);
    AnsiConsole.MarkupLine("[green]Session deleted successfully![/]");
    AnsiConsole.MarkupLine("Press Any Key to Continue.");
    Console.ReadKey();
  }
}