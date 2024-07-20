using System.Diagnostics;

namespace CodingTracker.kwm0304.Models;

public class CodingSession
{
  public int Id { get; set; }
  public DateTime StartTime { get; set; }
  public DateTime EndTime { get; set; }
  public TimeSpan SessionLength { get; set; }
  public Stopwatch? _stopWatch;

  public CodingSession()
  {
    _stopWatch = new Stopwatch();
  }
  public CodingSession(DateTime startTime, DateTime endTime, TimeSpan sessionLength)
  {
    StartTime = startTime;
    EndTime = endTime;
    SessionLength = sessionLength;
    _stopWatch = new Stopwatch();
  }

  public void StartSession()
  {
    StartTime = DateTime.Now;
    _stopWatch!.Reset();
    _stopWatch.Start();
  }

  public void EndSession()
  {
    _stopWatch!.Stop();
    EndTime = DateTime.Now;
    SessionLength = _stopWatch.Elapsed;
  }

  public override string ToString()
  {
    return $"Session ID: {Id}, Start: {StartTime}, End: {EndTime}, Duration: {SessionLength}";
  }
}
