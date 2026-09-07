using CodingTracker.DzemalKurtic.Controllers;
using Spectre.Console;

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
                .Title("What do you want to do?")
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

        table.AddColumn("ID");
        table.AddColumn("Start Time");
        table.AddColumn("End Time");
        table.AddColumn("Duration");

        var sessions = _codingSessionController.ViewItems();

        foreach (var session in sessions)
        {
            table.AddRow(
                session.Id.ToString(),
                $"[cyan]{session.StartTime:dd-MM-yyyy HH:mm}[/]",
                $"[yellow]{session.EndTime:dd-MM-yyyy HH:mm}[/]",
                $"[green]{session.Duration.Hours} hours {session.Duration.Minutes} minutes[/]"
                );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }

    internal void AddItem()
    {
        var start = getDate("start");
        var startDate = Validation.ValidateDate(start, "start");

        var end = getDate("end");
        var endDate = Validation.ValidateDate(end, "end");

        var isBigger = Validation.ValidateTimespan(startDate, endDate);

        if (!isBigger)
        {
            _codingSessionController.AddItem(startDate, endDate);
            AnsiConsole.MarkupLine("Press Any Key to Continue.");
            Console.ReadKey();
        }
        else
        {
            AnsiConsole.MarkupLine("Press Any Key to Continue.");
            Console.ReadKey();
            
            Console.Clear();
            AddItem();
        }
     
    }

    internal void UpdateItem()
    {
        ShowItems();

        var itemId = getId("update");
        var id = Validation.ValidateId(itemId);

        var start = getDate("start");
        var startDate = Validation.ValidateDate(start, "start");

        var end = getDate("end");
        var endDate = Validation.ValidateDate(end, "end");

        var rowCount = _codingSessionController.UpdateItem(itemId, startDate, endDate);
        if (rowCount == 0)
        {
            AnsiConsole.MarkupLine($"Session with id {id} doesn't exist.");
            AnsiConsole.MarkupLine("Press Any Key to Continue.");
            Console.ReadKey();
            Console.Clear();
            MainMenu();
        }else
        {
            AnsiConsole.MarkupLine("Press Any Key to Continue.");
            Console.ReadKey();
        }
    }

    internal void DeleteItem()
    {
        ShowItems();

        var itemId = getId("delete");
        var id = Validation.ValidateId(itemId);

        var rowCount = _codingSessionController.DeleteItem(id);
        if (rowCount == 0)
        {
            AnsiConsole.MarkupLine($"Session with id {id} doesn't exist.\n");
            AnsiConsole.MarkupLine("Press Any Key to Continue.\n");
            Console.ReadKey();
            Console.Clear();
            DeleteItem();
        }
        else
        {
            AnsiConsole.MarkupLine("Press Any Key to Continue.");
            Console.ReadKey();
        }
    }

    private string getDate(string time)
    {
        return AnsiConsole.Ask<string>($"Enter the {time} time of the Coding Session: (Format: dd-mm-yy HH:mm)");
    }

    private int getId(string action)
    {
        return AnsiConsole.Ask<int>($"Please typt the Id od the item you want to {action}");
    }
}
