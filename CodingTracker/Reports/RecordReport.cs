using Spectre.Console;

internal class RecordReport
{
    internal static void ShowReportForPeriodOfTime()
    {
        AnsiConsole.MarkupLine($"Creating report for period of time.");
        AnsiConsole.MarkupLine("[yellow]Start date input:[/]");

        DateTime startDate = SessionDate.GetDate();
        var (isValid, minDate) = ReportCrud.GetMinDateFromDb();
        if (!isValid) return;

        while (startDate < minDate)
        {
            AnsiConsole.MarkupLine("[red]Entered can not be earlier than first session's date in the database.[/]\n" +
                $"First record's date in database is [yellow]{minDate.ToString(Config.DateFormat)}[/].");
            var answer = DisplayInfoHelpers.GetYesNoAnswer("Do you want to use it as a starting date for report?");
            if (answer)
            {
                startDate = minDate;
                break;
            }
            else startDate = SessionDate.GetDate();
        }
        AnsiConsole.MarkupLine($"Report start date: {startDate.ToString(Config.DateFormat)}\n");

        AnsiConsole.MarkupLine("[yellow]End date input:[/]");
        DateTime endDate = SessionDate.GetDate();
        while (endDate < startDate)
        {
            AnsiConsole.MarkupLine($"[red]Entered ending date \"{endDate.ToString(Config.DateFormat)}\" " +
                $"can not be earlier than starting date.[/]");
            endDate = SessionDate.GetDate();
        }
        AnsiConsole.MarkupLine($"Report end date: {endDate.ToString(Config.DateFormat)}\n");

        if (!ShowTotalAverageInfo(startDate, endDate)) return;
        DisplayInfoHelpers.PressAnyKeyToContinue();
    }

    internal static bool ShowTotalAverageInfo(DateTime startDate, DateTime endDate)
    {
        if (endDate == DateTime.Today) endDate = DateTime.Now;
        else endDate += new TimeSpan(23, 59, 59);

        var durationReport = ReportCrud.GetDurationInfo(startDate, endDate);
        if (!durationReport.HasData) return false;

        var reportInfo = new ReportInfo
        {
            StartDate = startDate,
            EndDate = endDate,
            TotalDuration = durationReport.TotalDuration,
            AverageDuration = durationReport.AverageDuration,
            TotalDays = durationReport.TotalDays
        };

        DisplayReportMessage(reportInfo);
        return true;
    }

    internal static void DisplayReportMessage(ReportInfo reportInfo)
    {
        string total = SessionDuration.GetTotalSessionDurationInfo(reportInfo.TotalDuration);
        AnsiConsole.MarkupLine(
            $"Total coding sessions duration for the period:\nFrom " +
            $"[indianred1_1]{reportInfo.StartDate.ToString(Config.DateFormat)}[/] to " +
            $"[indianred1]{reportInfo.EndDate.ToString(Config.DateFormat)}[/] " +
            $"[indianred1]{reportInfo.EndDate.ToString(Config.TimeFormat)}[/] " +
            $"=> [chartreuse3_1]{total}[/].");

        string average = SessionDuration.GetTotalSessionDurationInfo(reportInfo.AverageDuration);
        AnsiConsole.MarkupLine($"\nAverage coding session duration per day for [indianred1_1]{reportInfo.TotalDays}[/] days: " +
            $"=> [chartreuse3_1]{average}[/].");
    }
}
