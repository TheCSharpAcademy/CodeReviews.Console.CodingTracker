namespace CodingTracker.MartinL_no.Models;

internal class PeriodStatistic
{
    public readonly Period Period;
    public readonly DateOnly StartDate;
    public readonly TimeSpan Total;
    public readonly TimeSpan Average;

    public PeriodStatistic(Period period, DateOnly startDate, TimeSpan total, TimeSpan average)
    {
        Period = period;
        StartDate = startDate;
        Total = total;
        Average = average;
    }
}