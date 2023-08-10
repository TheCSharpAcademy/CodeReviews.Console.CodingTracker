namespace CodingTracker.MartinL_no.Models;

internal class CodingStatistic
{
    public readonly PeriodType PeriodType;
    public readonly DateTime StartDate;
    public readonly TimeSpan Total;
    public readonly TimeSpan Average;

    public CodingStatistic(PeriodType periodType, DateTime startDate, TimeSpan total, TimeSpan average)
    {
        PeriodType = periodType;
        StartDate = startDate;
        Total = total;
        Average = average;
    }
}