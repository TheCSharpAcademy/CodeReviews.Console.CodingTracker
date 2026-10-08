using System.Globalization;
using Spectre.Console;

namespace CodingTracker;

public class CodingSession
{
    private DateTime StartTime { get; set; }
    private DateTime EndTime { get; set; }
    private TimeSpan Duration { get; set; }

    public CodingSession(DateTime startTime, DateTime endTime, TimeSpan duration)
    {
        StartTime = startTime;
        EndTime = endTime;
        Duration = duration;
    }

    public void AddRowToTable(Table table)
    {
        table.AddRow(
            StartTime.ToString(CultureInfo.CurrentCulture),
            EndTime.ToString(CultureInfo.CurrentCulture),
            Duration.ToString());
    }
}