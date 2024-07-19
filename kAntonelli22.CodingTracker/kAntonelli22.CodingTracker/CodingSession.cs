using System.Globalization;

namespace CodingTracker;
internal class CodingSession
{
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public TimeSpan duration { get; set; }
    public static List<CodingSession> sessions { get; set; } = new List<CodingSession>();

    public CodingSession(DateTime start, DateTime end)
    {
        this.start = start;
        this.end = end;
        this.duration = CalcDuration();
        sessions.Add(this);
    } // end of CodingSession Constructor
    public CodingSession(string start, string end, string duration)
    {
        this.start = DateTime.Parse(start, CultureInfo.InvariantCulture);
        this.end = DateTime.Parse(end, CultureInfo.InvariantCulture);
        this.duration = TimeSpan.Parse(duration, CultureInfo.InvariantCulture);
        // sessions.Add(this); 
    } // end of CodingSession Constructor for DatabaseManager  

    private TimeSpan CalcDuration()
    {
        return end - start;
    }
} // end of CodingSession Class
