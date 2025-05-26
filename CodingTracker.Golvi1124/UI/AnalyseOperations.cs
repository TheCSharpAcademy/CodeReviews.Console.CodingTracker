using CodingTracker.Golvi1124.Data;
using CodingTracker.Golvi1124.Helpers;
using CodingTracker.Golvi1124.Models;
using Spectre.Console;
using static CodingTracker.Golvi1124.UI.Enums;

namespace CodingTracker.Golvi1124.UI;

internal class AnalyseOperations
{
    internal static void FilterPerPeriod(IEnumerable<CodingRecord> records)
    {
        Console.Clear();

        if (!records.Any())
        {
            AnsiConsole.MarkupLine("[red]No records found in the system.[/]");
            return;
        }

        var oldestRecord = records.Any() ? records.Min(r => r.DateStart) : DateTime.MinValue;
        var newestRecord = records.Any() ? records.Max(r => r.DateStart) : DateTime.MaxValue;



        AnsiConsole.Markup("Now you need to choose [blue]period[/] you want to see records from.");
        AnsiConsole.MarkupLine($"In system, records are from [green]{oldestRecord:dd-MM-yy}[/] to [green]{newestRecord:dd-MM-yy}[/]");


        // Prompt for start date
        DateTime startDate = Validation.GetValidDate(
            "Enter the [blue]start[/] date (dd-MM-yy):",
            date => date.Date >= oldestRecord.Date && date.Date <= newestRecord.Date
        );

        // Prompt for end date
        DateTime endDate = Validation.GetValidDate(
            "Enter the [blue]end[/] date (dd-MM-yy):",
            date => date.Date >= startDate.Date && date.Date <= newestRecord.Date
        );

        // Ask for sort order
        bool isDescending = Extras.AskSortDirection();

        // Filter and sort records
        var filtered = isDescending
            ? records
                .Where(r => r.DateStart.Date >= startDate.Date && r.DateStart.Date <= endDate.Date)
                .OrderByDescending(r => r.DateStart)
                .ToList()
            : records
                .Where(r => r.DateStart.Date >= startDate.Date && r.DateStart.Date <= endDate.Date)
                .OrderBy(r => r.DateStart)
                .ToList();


        AnsiConsole.MarkupLine($"[green]Found {filtered.Count} records between {startDate:dd-MM-yy} and {endDate:dd-MM-yy}[/]");

        if (filtered.Any())
        {
            RecordOperations.ViewRecords(filtered);
        }
        else
        {
            AnsiConsole.MarkupLine("[red]No records found in the selected date range.[/]");
        }
    }

    internal static void ViewTotalAndAverage()
    {
        Console.Clear();
        var allRecords = new DataAccess().GetAllRecords();

        PeriodType period = Extras.AskPeriodType();

        if (period == PeriodType.Back)
            return;

        var range = Extras.AskDateRangeForPeriod(period);

        var recordsInRange = allRecords
            .Where(r => r.DateStart >= range.Start && r.DateStart <= range.End)
            .ToList();

        Console.Clear();

        if (!recordsInRange.Any())
        {
            AnsiConsole.MarkupLine($"[red]No records found between {range.Start:dd-MM-yy} and {range.End:dd-MM-yy}.[/]");
            return;
        }

        int sessionCount = recordsInRange.Count;
        double totalMinutes = recordsInRange.Sum(r => r.Duration.TotalMinutes);
        TimeSpan totalDuration = TimeSpan.FromMinutes(totalMinutes);

        double averagePerSessionMinutes = totalMinutes / sessionCount;
        TimeSpan averagePerSession = TimeSpan.FromMinutes(averagePerSessionMinutes);

        double periodDays = (range.End - range.Start).TotalDays + 1;
        double averagePerDayMinutes = totalMinutes / periodDays;
        TimeSpan averagePerDay = TimeSpan.FromMinutes(averagePerDayMinutes);

        AnsiConsole.MarkupLine($"[blue]In selected period ({range.Start:dd-MM-yy} to {range.End:dd-MM-yy}):[/]");
        AnsiConsole.MarkupLine($"[green]• Sessions:[/] {sessionCount}");
        AnsiConsole.MarkupLine($"[green]• Total time:[/] {totalDuration.Hours}h {totalDuration.Minutes % 60}m");
        AnsiConsole.MarkupLine($"[green]• Avg per session:[/] {averagePerSession.Hours}h {averagePerSession.Minutes % 60}m");
        AnsiConsole.MarkupLine($"[green]• Avg per day:[/] {averagePerDay.Hours}h {averagePerDay.Minutes % 60}m");

        Console.WriteLine();
        AnsiConsole.MarkupLine("[grey]Press any key to return to the Analyse Menu...[/]");
        Console.ReadKey();
    }

    internal static void IsGoalReached()
    {
        Console.Clear();

        var allRecords = new DataAccess().GetAllRecords();
        PeriodType period = Extras.AskPeriodType();

        if (period == PeriodType.Back)
            return;

        var range = Extras.AskDateRangeForPeriod(period);

        var recordsInRange = allRecords
            .Where(r => r.DateStart >= range.Start && r.DateStart <= range.End)
            .ToList();

        Console.Clear();

        if (!recordsInRange.Any())
        {
            AnsiConsole.MarkupLine($"[red]No coding sessions found between {range.Start:dd-MM-yy} and {range.End:dd-MM-yy}.[/]");
            Console.ReadKey();
            return;
        }

        // Ask for user's daily goal
        int goalHours = AnsiConsole.Ask<int>("How many [green]hours[/] per day did you want to code?");
        int goalMinutes = AnsiConsole.Ask<int>("How many [green]additional minutes[/] per day?");
        var goalPerDay = TimeSpan.FromMinutes(goalHours * 60 + goalMinutes);

        // Calculate total coded time
        double totalMinutes = recordsInRange.Sum(r => r.Duration.TotalMinutes);
        double daysInPeriod = (range.End - range.Start).TotalDays + 1;
        var actualPerDay = TimeSpan.FromMinutes(totalMinutes / daysInPeriod);

        double progressPercent = actualPerDay.TotalMinutes / goalPerDay.TotalMinutes * 100;

        Console.Clear();

        AnsiConsole.MarkupLine($"[bold]Selected period:[/] [blue]{range.Start:dd-MM-yy} to {range.End:dd-MM-yy}[/]");
        AnsiConsole.MarkupLine($"[bold]Your goal:[/] {goalPerDay.Hours}h {goalPerDay.Minutes}m per day");
        AnsiConsole.MarkupLine($"[bold]Your result:[/] {actualPerDay.Hours}h {actualPerDay.Minutes}m per day");

        if (actualPerDay >= goalPerDay)
        {
            TimeSpan over = actualPerDay - goalPerDay;

            AnsiConsole.MarkupLine($"\n[green]Congratulations![/]");
            AnsiConsole.MarkupLine($"You exceeded your goal by [green]{over.Hours}h {over.Minutes}m[/] per day.");
            AnsiConsole.MarkupLine($"[bold]Progress:[/] [green]{progressPercent:F1}%[/]");
        }
        else
        {
            TimeSpan missing = goalPerDay - actualPerDay;

            AnsiConsole.MarkupLine($"\n[yellow]Keep going![/]");
            AnsiConsole.MarkupLine($"You were short by [red]{missing.Hours}h {missing.Minutes}m[/] per day.");
            AnsiConsole.MarkupLine($"[bold]Progress:[/] [yellow]{progressPercent:F1}%[/]");
        }

        Console.WriteLine();
        AnsiConsole.MarkupLine("[grey]Press any key to return to the Analyse Menu...[/]");
        Console.ReadKey();
    }
}