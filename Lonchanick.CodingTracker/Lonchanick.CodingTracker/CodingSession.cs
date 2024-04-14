namespace Lonchanick.CodingTracker;

internal class CodingSession
{
    public int Id { get; set; }
    public DateTime DateTimeSessionInit { get; set; }
    public DateTime DateTimeSessionEnd { get; set; }
    public TimeSpan Duration { get; set; }

}
