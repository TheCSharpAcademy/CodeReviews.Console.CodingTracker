using Serilog;

namespace CodingTracker.barakisbrown;

public class CodingSession
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration { get; set; }

    public override string ToString()
    {
        var retString = $"ID = {Id}\tStartTime = {StartTime}\tEndTime = {EndTime}\tDuration = {Duration}";
        Log.Information("F> CodingSession: toString => {0}", retString);
        return retString;
    }

    public TimeSpan CalculateDuration(TimeOnly begin, TimeOnly end) => end - begin;

    public void CombineDTSeperated(DTSeperated begin, DTSeperated end)
    {
        throw new NotImplementedException();
    }
}

public struct DTSeperated
{
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
}