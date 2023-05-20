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
        var retString = $"StartTime = {StartTime}\tEndTime = {EndTime} \t Duration = {Duration}";
        Log.Information("F> CodingSession: toString => {0}", retString);
        return retString;
    }

    public static TimeSpan CalculateDuration(TimeOnly begin, TimeOnly end) => end - begin;

    public void CombineDTSeperated(DTSeperated begin, DTSeperated end)
    {
        StartTime = begin.Date.ToDateTime(begin.Time);
        EndTime = end.Date.ToDateTime(end.Time);
        Duration = CalculateDuration(begin.Time, end.Time);
    }

    public DTSeperated SeperateBegin() 
    {
        return new()
        {
            Date = DateOnly.FromDateTime(StartTime),
            Time = TimeOnly.FromDateTime(StartTime)
        };
    }
    public DTSeperated SeperateEnd() 
    {
        return new()
        {
            Date = DateOnly.FromDateTime(EndTime),
            Time = TimeOnly.FromDateTime(EndTime)
        };
    }

    public void ValidateTime(DTSeperated begin, DTSeperated end)
    {
        // NEED TO MAKE SURE THE BEGIN TIME IS NOT GREATER THAN THE END TIME WHILE DATE IS THE SAME
        // TEST IF BEGIN > END AND END LESS THAN BEGIN 
        var exitFlag = true;
        while (exitFlag)
        {
            if ((begin.Time >= end.Time) && (begin.Date == end.Date))
            {
                Console.WriteLine("\nBegin time can not be equal or greater than the end time.  Please re-enter the begin time.");
                begin.Time = Input.GetTime();
            }
            else if (end.Time < begin.Time && end.Date == begin.Date)
            {
                Console.WriteLine("\nEnd Time can not be less than the begin time.");
                Console.WriteLine("Please re-enter the end time");
                end.Time = Input.GetTime();
            }
            else
                exitFlag = false;
        }
    }

    public void ValidateDate(DTSeperated begin, DTSeperated end)
    {
        var exitFlag = true;
        while (exitFlag)
        {
            if (end.Date < begin.Date)
            {
                Console.WriteLine("\nYou can not have a date in the past for the end session. Please try again.");
                end.Date = Input.GetDate();
            }
            else
                exitFlag = false;
        }
    }
}

public struct DTSeperated
{
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }

    public override string ToString()
    {
        return $"{Date}\t{Time}";
    }
}