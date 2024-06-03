namespace CodingTracker.Models;

#region ReportsModels
internal class CodingReport
{
    public string CodingTime { get; set; } = default!;
    public string AverageTime { get; set; } = default!;
    public string TotalSessions { get; set; } = default!;
}

internal class YearlyReport : CodingReport
{
    public int Year { get; set; }
}

internal class MonthlyReport : CodingReport
{
    public int Year { get; set; }
    public int Month { get; set; }
}
internal class WeeklyReport : CodingReport
{
    public int Year { get; set; }
    public int Week { get; set; }
}

internal class DailyReport : CodingReport
{
    public string Date { get; set; } = default!;
}

#endregion

#region ReportsDtos

internal class ReportDto
{
    public string StartDate { get; set; } = default!;
    public string EndDate { get; set; } = default!;
    public string Sort = "asc";
}

#endregion