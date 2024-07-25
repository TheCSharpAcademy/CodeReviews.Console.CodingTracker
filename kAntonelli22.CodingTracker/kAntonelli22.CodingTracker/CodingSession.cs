using System.Globalization;

namespace CodingTracker;
internal class CodingSession
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public TimeSpan Duration { get; set; }
    public static List<CodingSession> Sessions { get; set; } = new List<CodingSession>();

    public CodingSession(DateTime Start, DateTime End)
    {
        this.Start = Start;
        this.End = End;
        Duration = CalcDuration();
        Sessions.Add(this);
    } // end of CodingSession Constructor

    public CodingSession(string Start, string End, string Duration)
    {
        this.Start = DateTime.Parse(Start, CultureInfo.InvariantCulture);
        this.End = DateTime.Parse(End, CultureInfo.InvariantCulture);
        this.Duration = TimeSpan.Parse(Duration, CultureInfo.InvariantCulture);
    } // end of CodingSession Constructor for DatabaseManager  

    private TimeSpan CalcDuration()
    {
        return End - Start;
    }
} // end of CodingSession Class
