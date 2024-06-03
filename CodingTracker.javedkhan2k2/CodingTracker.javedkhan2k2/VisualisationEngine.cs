
using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker;

internal static class VisualisationEngine
{
    internal static void DisplayAllSessions(IEnumerable<CodingSession>? sessions)
    {
        AnsiConsole.Clear();
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.Collapse();
        table.ShowRowSeparators = true;
        table.Title = new TableTitle("Coding Sessions List", new Style(Color.CadetBlue, Color.Grey100, Decoration.Bold));
        table.AddColumns(["Id", "Start Time", "End Time", "Duration (HH:mm:ss)"]);
        foreach (var session in sessions)
        {
            table.AddRow(session.Id.ToString(), session.StartTime.ToString(), session.EndTime.ToString(), session.Duration.ToString());
        }
        AnsiConsole.Write(table);
    }

    internal static void DisplayDailyReport(IEnumerable<DailyReport>? rows, ReportDto input)
    {
        AnsiConsole.Clear();
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.Collapse();
        table.ShowRowSeparators = true;
        table.Title = new TableTitle($"Daily Report from [blue]{input.StartDate} to {input.EndDate}[/]", new Style(Color.CadetBlue, Color.Grey100, Decoration.Bold));
        table.AddColumns(["#", "Date", "Coding Time in Hours", "Average", "Total Sessions"]);
        int serial = 1;
        foreach (var row in rows)
        {
            table.AddRow(serial.ToString(), row.Date, row.CodingTime, row.AverageTime, row.TotalSessions);
            serial += 1;
        }
        AnsiConsole.Write(table);
    }
    
    internal static void DisplayWeeklyReport(IEnumerable<WeeklyReport>? rows, ReportDto input)
    {
        AnsiConsole.Clear();
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.Collapse();
        table.ShowRowSeparators = true;
        table.Title = new TableTitle($"Weekly Report from [blue]{input.StartDate} to {input.EndDate}[/]", new Style(Color.CadetBlue, Color.Grey100, Decoration.Bold));
        table.AddColumns(["#", "Year", "Week #", "Coding Time in Hours", "Average", "Total Sessions"]);
        int serial = 1;
        foreach (var row in rows)
        {
            table.AddRow(serial.ToString(), row.Year.ToString(), row.Week.ToString(), row.CodingTime, row.AverageTime, row.TotalSessions);
            serial += 1;
        }
        AnsiConsole.Write(table);
    }
    
    internal static void DisplayMonthlyReport(IEnumerable<MonthlyReport>? rows, ReportDto input)
    {
        AnsiConsole.Clear();
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.Collapse();
        table.ShowRowSeparators = true;
        table.Title = new TableTitle($"Monthly Report from [blue]{input.StartDate} to {input.EndDate}[/]", new Style(Color.CadetBlue, Color.Grey100, Decoration.Bold));
        table.AddColumns(["#", "Year", "Month #", "Coding Time in Hours", "Average", "Total Sessions"]);
        int serial = 1;
        foreach (var row in rows)
        {
            table.AddRow(serial.ToString(), row.Year.ToString(), row.Month.ToString(), row.CodingTime, row.AverageTime, row.TotalSessions);
            serial += 1;
        }
        AnsiConsole.Write(table);
    }

    internal static void DisplayYearlyReport(IEnumerable<YearlyReport>? rows, ReportDto input)
    {
        AnsiConsole.Clear();
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.Collapse();
        table.ShowRowSeparators = true;
        table.Title = new TableTitle($"Yearly Report from [blue]{input.StartDate} to {input.EndDate}[/]", new Style(Color.CadetBlue, Color.Grey100, Decoration.Bold));
        table.AddColumns(["#", "Year", "Coding Time in Hours", "Average", "Total Sessions"]);
        int serial = 1;
        foreach (var row in rows)
        {
            table.AddRow(serial.ToString(), row.Year.ToString(), row.CodingTime, row.AverageTime, row.TotalSessions);
            serial += 1;
        }
        AnsiConsole.Write(table);
    }

    internal static void DisplaySessionDto(CodingSessionDto codingSessionDto)
    {
        AnsiConsole.Markup("{0,-20} {1,-20}\n", "Start Time", $"[blue]{codingSessionDto.StartTime}[/]");
        AnsiConsole.Markup("{0,-20} {1,-20}\n", "End Time", $"[blue]{codingSessionDto.EndTime}[/]");
        AnsiConsole.Markup("{0,-20} {1,-20}\n", "Duration", $"[blue]{codingSessionDto.Duration}[/]");
    }

    internal static void DisplayInvalidDateInputError(string startTime, string endTime)
    {
        if (!Validation.IsValidDateTimeInputs(startTime, endTime))
        {
            AnsiConsole.Markup($"End Date Time [bold green]{endTime}[/] should be later than Start Date Time [bold green]{startTime}[/]\n");
        }
        else
        {
            AnsiConsole.Markup($"Enter valid Start and End Date Time");
        }
        AnsiConsole.Markup("Press [green]Enter[/] to continue");
        Console.ReadLine();
    }

    internal static void DisplayReportDto(ReportDto reportInput)
    {
        AnsiConsole.Markup("{0,-20} {1,-20}\n", "Start Date", $"[blue]{reportInput.StartDate}[/]");
        AnsiConsole.Markup("{0,-20} {1,-20}\n", "End Date", $"[blue]{reportInput.EndDate}[/]");
        AnsiConsole.Markup("{0,-20} {1,-20}\n", "Sort", $"[blue]{reportInput.Sort}[/]");
    }
    
}