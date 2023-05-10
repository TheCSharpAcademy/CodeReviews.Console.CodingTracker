using Serilog;

namespace CodingTracker.barakisbrown;

public class CodingSession
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime Duration { get; set; }

    public override string ToString()
    {
        var retString = $"ID = {Id}\tStartTime = {StartTime}\tEndTime = {EndTime}\tDuration = {Duration}";
        Log.Information("F> CodingSession: toString => {0}", retString);
        return retString;
    }
}
