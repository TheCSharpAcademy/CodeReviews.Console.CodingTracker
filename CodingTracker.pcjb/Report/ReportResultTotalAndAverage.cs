namespace CodingTracker;

class ReportResultTotalAndAverage
{
    public string Period { get; set; }
    public TimeSpan Total { get; set; }
    public TimeSpan Average { get; set; }

    public ReportResultTotalAndAverage(string period, TimeSpan total, TimeSpan average)
    {
        Period = period;
        Total = total;
        Average = average;
    }
}