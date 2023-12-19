namespace CodingTrackerLibrary;
public class CodingReportModel
{
    public List<CodingSessionModel> CodingReports { get; set; } = new();
    public int ReportCount 
    {
        get
        {
            return CodingReports.Count;
        } 
    }
    public TimeSpan Time 
    {
        get
        {
            TimeSpan total = TimeSpan.Zero;
            foreach (CodingSessionModel c in CodingReports)
            {
                total += c.Duration;
            }
            return total;
        }        
    }
    public TimeSpan Average 
    {
        get
        {
            return Time / ReportCount;
        } 
    }

    public CodingReportModel()
    {
        
    }

    public CodingReportModel(List<CodingSessionModel> sessions)
    {
        CodingReports = sessions;
    }
}
