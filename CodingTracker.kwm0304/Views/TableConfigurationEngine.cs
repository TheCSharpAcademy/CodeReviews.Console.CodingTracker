using System.Diagnostics;
using CodingTracker.kwm0304.Models;
using Spectre.Console;

namespace CodingTracker.kwm0304.Views;

public class TableConfigurationEngine
{
  private static string header = @"
   ______          ___                ______                __            
  / ____/___  ____/ (_)___  ____ _   /_  __/________ ______/ /_____  _____
 / /   / __ \/ __  / / __ \/ __ `/    / / / ___/ __ `/ ___/ //_/ _ \/ ___/
/ /___/ /_/ / /_/ / / / / / /_/ /    / / / /  / /_/ / /__/ ,< /  __/ /    
\____/\____/\__,_/_/_/ /_/\__, /    /_/ /_/   \__,_/\___/_/|_|\___/_/     
                         /____/                                           
"
;

  public static string MainMenu()
  {
    AnsiConsole.WriteLine(header);
    return AnsiConsole.Prompt(
      new SelectionPrompt<string>()
      .AddChoices("Start a new session", "View past sessions", "Generate reports", "Exit")
    );
  }

  public static async Task LiveSessionDisplay()
  {
    var stopWatch = new Stopwatch();
    stopWatch.Start();

    await AnsiConsole.Live(new Panel(new Markup("[bold]Timer:[/] [yellow]0:00:00[/]"))
      .Header("[green]Coding Session Timer[/]")
      .Collapse()
      .RoundedBorder())
      .StartAsync(async ctx =>
      {
        while (true)
        {
          var elapsed = stopWatch.Elapsed;
          ctx.UpdateTarget(new Panel(new Markup($"[bold]Timer:[/] [yellow]{elapsed:hh\\:mm\\:ss}[/]"))
          .Header("[green]Coding Session Timer[/]")
          .Collapse()
          .RoundedBorder());
          await Task.Delay(1000);
          if (Console.KeyAvailable)
          {
            var selection = AnsiConsole.Prompt(
              new SelectionPrompt<string>()
              .AddChoices("Done"));
            if (selection == "Done")
            {
              stopWatch.Stop();
            }
          }
        }
      });
    var total = stopWatch.Elapsed;
    AnsiConsole.WriteLine($"Session ended. Duration: {total.TotalSeconds} seconds");
  }

    public static string DisplayReportOptions()
    {
        throw new NotImplementedException();
    }

}