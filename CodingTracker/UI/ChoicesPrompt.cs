using System.Runtime.CompilerServices;
using Spectre.Console;
using static CodingTracker.TimeFormatter;

namespace CodingTracker;

internal class ChoicesPrompt
{
  internal static string AskForActivity(string action, string colorHighlight)
  {
    AnsiConsole.MarkupLine("[grey]Press 'Enter' to autofill with the last activity registered.[/]");
    var prompt = new TextPrompt<string>($"[{colorHighlight}]{action}[/] activity:").AllowEmpty();
    return AnsiConsole.Prompt(prompt);
  }

  public static List<CodingSession> AskForFilter(string action, string colorHighlight)
  {
    List<CodingSession> tableData = new();
    string[] choices = [
      $"{action} from all",
      $"{action} from specific {Enums.Column.activity.GetDisplayName()}",
      $"{action} from specific Time Range"];

    var selection = AnsiConsole.Prompt(
      new SelectionPrompt<string>()
      .Title($"What do you want to [BOLD {colorHighlight}]{action}[/]")
      .HighlightStyle(colorHighlight)
      .PageSize(100)
      .Mode(SelectionMode.Leaf)
      .AddChoices(choices));

    if (selection == choices[0])
    {
      tableData = Query.ViewAll();
    }
    else if (selection == choices[1])
    {
      string activity = AskForActivity(action, colorHighlight);
      tableData = Query.FilterByActivity(activity);
    }
    else if (selection == choices[2])
    {
      string startDate = ConvertDateForDb(GetTimeFromUser(DaTeStyleShort, "Search from", "00:00"));
      string endDate = ConvertDateForDb(GetTimeFromUser(DaTeStyleShort, "Till", "23:59"));
      tableData = Query.FilterByDate(startDate, endDate);
    }

    return tableData;
  }

  public static CodingSession SelectEntry(List<CodingSession> tableData, string action, string colorHighlight)
  {
    var selection = AnsiConsole.Prompt(
    new SelectionPrompt<CodingSession>()
    .Title($"[BOLD {colorHighlight}]Select[/] the entry you want to [BOLD {colorHighlight}]{action}[/]:")
    .HighlightStyle(colorHighlight)
    .UseConverter(entry => entry.DisplayData())
    .PageSize(100)
    .AddChoices(tableData));

    return selection;
  }

  public static List<string> SelectMultiString(string action, string colorHighlight, string[] choices)
  {
    var selection = AnsiConsole.Prompt(
      new MultiSelectionPrompt<string>()
      .Title($"What do you want to [BOLD {colorHighlight}]{action}[/]")
      .HighlightStyle(colorHighlight)
      .InstructionsText($"[grey](Press [{colorHighlight}]<space>[/] to toggle, [green]<enter>[/] to confirm)[/]")
      .AddChoices(choices));

    return selection;
  }

  public static List<CodingSession> SelectMultiSessions(string action, string colorHighlight, List<CodingSession> tableData)
  {
    var selection = AnsiConsole.Prompt(
    new MultiSelectionPrompt<CodingSession>()
    .Title($"[BOLD {colorHighlight}]Select[/] the entry you want to [BOLD {colorHighlight}]{action}[/]:")
    .HighlightStyle(colorHighlight)
    .InstructionsText($"[grey](Press [{colorHighlight}]<space>[/] to toggle, [green]<enter>[/] to confirm)[/]")
    .UseConverter(entry => entry.DisplayData())
    .AddChoices(tableData));

    return selection;
  }
}