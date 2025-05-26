using CodingTracker.Golvi1124.Data;
using CodingTracker.Golvi1124.Helpers;
using CodingTracker.Golvi1124.Models;
using CodingTracker.Golvi1124.Services;
using Spectre.Console;

namespace CodingTracker.Golvi1124.UI;

internal static class RecordOperations
{
    internal static void DeleteRecord()
    {
        var dataAccess = new DataAccess();
        var records = dataAccess.GetAllRecords();
        ViewRecords(records);

        var id = GetNumber("Please type the id of the habit you want to delete.");

        if (!AnsiConsole.Confirm("Are you sure?"))
            return;

        var response = dataAccess.DeleteRecord(id);

        var responseMessage = response < 1
            ? $"Record with the id {id} doesn't exist. Press any key to return to Main Menu"
            : "Record deleted successfully. Press any key to return to Main Menu";

        System.Console.WriteLine(responseMessage);
        System.Console.ReadKey();
    }

    internal static void UpdateRecord()
    {
        var dataAccess = new DataAccess();
        var records = dataAccess.GetAllRecords();
        ViewRecords(records);

        var id = GetNumber("Please type the id of the habit you want to update.");

        var record = records.Where(x => x.Id == id).Single();
        var dates = GetDateInputs();

        record.DateStart = dates[0];
        record.DateEnd = dates[1];

        dataAccess.UpdateRecord(record);
    }

    internal static int GetNumber(string message)
    {
        string numberInput = AnsiConsole.Ask<string>(message);

        if (numberInput == "0") UserInterface.MainMenu();

        var output = Validation.ValidateInt(numberInput, message);

        return output;
    }

    internal static void ViewRecords(IEnumerable<CodingRecord> records)
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Start Date");
        table.AddColumn("End Date");
        table.AddColumn("Duration");

        foreach (var record in records)
        {
            var hours = (int)record.Duration.TotalHours;
            var minutes = (int)Math.Round(record.Duration.TotalMinutes % 60);

            if (minutes == 60)
            {
                hours++;
                minutes = 0;
            }

            table.AddRow(
                record.Id.ToString(),
                record.DateStart.ToString("dd-MMM-yy HH:mm:ss"),
                record.DateEnd.ToString("dd-MMM-yy HH:mm:ss"),
                $"{hours} hours {minutes} minutes"
            );
        }
        AnsiConsole.Write(table);
    }

    internal static void AddRecord()
    {
        CodingRecord record = new();

        var dateInputs = GetDateInputs();
        record.DateStart = dateInputs[0];
        record.DateEnd = dateInputs[1];

        var dataAccess = new DataAccess();
        dataAccess.InsertRecord(record);
    }

    internal static DateTime[] GetDateInputs()
    {
        var startDateInput = AnsiConsole.Ask<string>("Input Start Date with the format: dd-mm-yy hh:mm (24 hour clock). Or enter 0 to return to main menu.");

        if (startDateInput == "0") UserInterface.MainMenu();

        var startDate = Validation.ValidateStartDate(startDateInput);

        var endDateInput = AnsiConsole.Ask<string>("Input End Date with the format: dd-mm-yy hh:mm (24 hour clock). Or enter 0 to return to main menu.");

        if (endDateInput == "0") UserInterface.MainMenu();

        var endDate = Validation.ValidateEndDate(startDate, endDateInput);

        return [startDate, endDate];
    }

    internal static void RunStopwatchSession()
    {
        var stopwatchService = new StopwatchService();
        var dataAccess = new DataAccess();

        var (start, end, duration) = stopwatchService.RunStopwatch();

        // Ensure that seconds are rounded up to the nearest minute
        if (duration.TotalMinutes < 1)
        {
            duration = TimeSpan.FromMinutes(1);
        }

        AnsiConsole.MarkupLine($"\n[bold yellow]Session summary:[/]");
        AnsiConsole.MarkupLine($"Start: [green]{start}[/]");
        AnsiConsole.MarkupLine($"End: [green]{end}[/]");
        AnsiConsole.MarkupLine($"Duration: [green]{(int)duration.TotalHours}h {(int)Math.Round(duration.TotalMinutes % 60)}m[/]");
        var saveSession = AnsiConsole.Confirm("\nDo you want to save this session?");

        if (saveSession)
        {
            CodingRecord record = new()
            {
                DateStart = start,
                DateEnd = end,
                Duration = duration
            };

            dataAccess.InsertRecord(record);
            AnsiConsole.MarkupLine("\n[green]Record saved successfully! Press any key to return to menu.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("\n[red]Session discarded. Press any key to return to menu.[/]");
        }

        Console.ReadKey();
    }
}