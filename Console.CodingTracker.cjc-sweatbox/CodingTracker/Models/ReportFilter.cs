using CodingTracker.Constants;
using CodingTracker.Enums;

namespace CodingTracker.Models;

/// <summary>
/// The filter to apply to a Report.
/// </summary>
public class ReportFilter
{
    #region Properties

    public ReportFilterType Type { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public ReportOrderByType OrderBy { get; set; }

    #endregion
    #region Methods

    public IEnumerable<CodingSessionReport> Apply(IEnumerable<CodingSession> items)
    {
        // Set default dates.
        if (!StartDate.HasValue)
        {
            StartDate = DateTime.MinValue;
        }
        
        if (!EndDate.HasValue)
        {
            EndDate = DateTime.MaxValue;
        }

        // Filter by Date.
        var filteredItems = items.Where(w => w.StartTime >= StartDate && w.EndTime <= EndDate);

        // Apply group by (day, week, month, year...)
        var groupedItems = Type switch
        {
            ReportFilterType.Day => filteredItems.GroupBy(g => new { StartDate = g.StartTime.Date, EndDate = g.StartTime.Date }).Select(s => new CodingSessionReport(s.Key.StartDate, s.Key.EndDate, s.Sum(sum => sum.Duration), StringFormat.Date)),
            ReportFilterType.Week => filteredItems.GroupBy(g => new { StartDate = g.StartTime.Date.AddDays(-(int)g.StartTime.DayOfWeek), EndDate = g.EndTime.Date.AddDays(-(int)g.StartTime.DayOfWeek + 6) }).Select(s => new CodingSessionReport(s.Key.StartDate, s.Key.EndDate, s.Sum(sum => sum.Duration), StringFormat.Date)),
            ReportFilterType.Month => filteredItems.GroupBy(g => new { StartDate = new DateTime(g.StartTime.Year, g.StartTime.Month, 1), EndDate = new DateTime(g.EndTime.Year, g.EndTime.Month, DateTime.DaysInMonth(g.EndTime.Year, g.EndTime.Month)) }).Select(s => new CodingSessionReport(s.Key.StartDate, s.Key.EndDate, s.Sum(sum => sum.Duration), StringFormat.YearMonth)),
            ReportFilterType.Year => filteredItems.GroupBy(g => new { StartDate = new DateTime(g.StartTime.Year, 1, 1), EndDate = new DateTime(g.EndTime.Year, 12, DateTime.DaysInMonth(g.EndTime.Year, 12)) }).Select(s => new CodingSessionReport(s.Key.StartDate, s.Key.EndDate, s.Sum(sum => sum.Duration), StringFormat.Year)),
            _ => filteredItems.Select(s => new CodingSessionReport(s.StartTime, s.EndTime, s.Duration, StringFormat.DateTime))
        };
        
        // Apply order by.
        var orderedItems = OrderBy switch
        {
            ReportOrderByType.Ascending => groupedItems.OrderBy(o => o.StartDateTime),
            ReportOrderByType.Descending => groupedItems.OrderByDescending(o => o.StartDateTime),
            _ => throw new Exception("Invalid ReportOrderByType")
        };

        // Return output.
        return orderedItems;
    }

    #endregion
}
