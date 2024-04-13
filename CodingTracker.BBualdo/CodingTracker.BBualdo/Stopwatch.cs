using Spectre.Console;

namespace CodingTracker.BBualdo;

public class Stopwatch
{
  public bool IsRunning { get; set; }
  public DateTime StartDate { get; set; }
  public DateTime StopDate { get; set; }

  public void Start()
  {
    if (!IsRunning)
    {
      IsRunning = true;
      StartDate = DateTime.Now;
    }
    else AnsiConsole.Markup("[red]Stopwatch is already running.[/]");
  }

  public void Stop()
  {
    if (IsRunning)
    {
      StopDate = DateTime.Now;
      IsRunning = false;
    }
    else AnsiConsole.Markup("[red]Start stopwatch first.[/]");
  }
}