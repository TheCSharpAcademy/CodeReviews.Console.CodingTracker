using CodingTracker.DzemalKurtic.Controllers;
using CodingTracker.DzemalKurtic.Models;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Globalization;

namespace CodingTracker.DzemalKurtic.Views;

internal class UserInterface
{
    private readonly CodingSessionController _codingSessionController;

    internal UserInterface(CodingSessionController controller)
    {
        _codingSessionController = controller;
    }

    internal void MainMenu()
    {
        bool appRunning = true;
        while (appRunning)
        {
            Console.Clear();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuAction>()
                .Title("What do you want to do next?")
                .AddChoices(Enum.GetValues<MenuAction>())
                .UseConverter(action => action switch
                {
                    MenuAction.ViewSessions => "View all Sessions",
                    MenuAction.AddSession => "Add a Session",
                    MenuAction.UpdateSession => "Update a Session",
                    MenuAction.DeleteSession => "Delete a Session",
                }));

            switch (choice)
            {
                case MenuAction.ViewSessions:
                    ShowItems();
                    break;
                case MenuAction.AddSession:
                    AddItem();
                    break;
                case MenuAction.UpdateSession:
                    UpdateItem();
                    break;
                case MenuAction.DeleteSession:
                    DeleteItem();
                    break;
            }
        }
    }

    internal void ShowItems()
    {
        var table = new Table();
        table.Border(TableBorder.Rounded);

        table.AddColumn("[yellow]ID[/]");
        table.AddColumn("[yellow]Start Time[/]");
        table.AddColumn("[yellow]End Time[/]");
        table.AddColumn("[yellow]Duration[/]");

        var sessions = _codingSessionController.ViewItems();

        foreach (var session in sessions)
        {
            table.AddRow(
                session.Id.ToString(),
                $"[cyan]{session.StartTime:dd-MM-yyyy HH:mm}[/]",
                $"[yellow]{session.EndTime:dd-MM-yyyy HH:mm}[/]",
                $"[green]{session.Duration.TotalHours:F2} hours[/]"
                );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }

    internal void AddItem()
    {
        var start = AnsiConsole.Ask<string>("Enter the start date of the Coding Session: (Format: dd-mm-yy HH:mm)");
        var end = AnsiConsole.Ask<string>("Enter the end date of the Coding Session: (format: dd-mm-yy HH:mm");

        var startDate = DateTime.ParseExact(start, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture);
        var endDate = DateTime.ParseExact(end, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture);

        _codingSessionController.AddItem(startDate, endDate);
        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }

    internal void UpdateItem()
    {
        ShowItems();

        var itemId = AnsiConsole.Ask<int>("Please typt the Id od the item you want to update");
        var start = AnsiConsole.Ask<string>("Enter the start date of the Coding Session: (Format: dd-mm-yy HH:mm)");
        var end = AnsiConsole.Ask<string>("Enter the end date of the Coding Session: (format: dd-mm-yy HH:mm");

        var startDate = DateTime.ParseExact(start, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture);
        var endDate = DateTime.ParseExact(end, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture);

        _codingSessionController.UpdateItem(itemId, startDate, endDate);
        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }

    internal void DeleteItem()
    {
        ShowItems();

        var itemId = AnsiConsole.Ask<int>("Please typt the Id od the item you want to delete");
        _codingSessionController.DeleteItem(itemId);
        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }
}
