using Spectre.Console;
using static CodingTracker.Enums;
using static CodingTracker.TimeFormatter;

namespace CodingTracker;

internal class UserInterface
{
  internal static void MainMenu()
  {
    bool exitApp = false;

    while (!exitApp)
    {
      var selection = AnsiConsole.Prompt(
          new SelectionPrompt<MenuChoice>()
          .Title("What do you want to do?")
          .HighlightStyle(Color.Grey)
          .UseConverter(choice => choice.GetDisplayName())
          .AddChoices(Enum.GetValues<MenuChoice>()));

      switch (selection)
      {
        case MenuChoice.viewItems:
          ViewAll();
          break;

        case MenuChoice.addItemStartNow:
          LiveSession.Launch();
          break;

        case MenuChoice.addItem:
          AddItem();
          break;

        case MenuChoice.editItem:
          EditItem();
          break;

        case MenuChoice.deleteItem:
          DeleteItem();
          break;

        case MenuChoice.exit:
          exitApp = true;
          break;
      }
    }
  }

  internal static void AddItem()
  {
    string colorHighlight = "green";

    AnsiConsole.MarkupLine($"[{colorHighlight}]Add Session[/]");

    string startDate = GetTimeFromUser(DaTeStyleShort, "Day").ToString(DaTeStyleShort);
    string startHour = GetTimeFromUser(DateStyleHour, "start hour").ToString(DateStyleHour);
    string endHour = GetTimeFromUser(DateStyleHour, "end hour").ToString(DateStyleHour);
    string activity = ChoicesPrompt.AskForActivity("Add", colorHighlight);

    string[] timeInfos = FormatTimeInfo(startDate, startHour, endHour);

    Database.Add(activity, timeInfos[0], timeInfos[1], Convert.ToInt32(timeInfos[2]));

    AnsiConsole.WriteLine();
    Displayer.PreviewRow(activity, timeInfos[0], timeInfos[1], Convert.ToInt32(timeInfos[2]));
    AnsiConsole.WriteLine();
    AnsiConsole.MarkupLine($"[{colorHighlight}]The session has been added to the DB.[/]");
    AnsiConsole.WriteLine();
  }

  internal static void ViewAll()
  {
    List<CodingSession> tableData = Query.ViewAll();
    Displayer.PrintTable(tableData, "[bold] all Sessions[/]");
  }

  internal static void EditItem()
  {
    Displayer.PrintTitle();

    if (Query.CountRows() == 0)
    {
      Displayer.NoEntriesFound();
      return;
    }

    string action = "Edit";
    string colorHighlight = "Blue";
    string[] timeInfos;

    List<CodingSession> tableData = ChoicesPrompt.AskForFilter(action, colorHighlight);
    AnsiConsole.WriteLine();

    if (tableData.Count == 0)
    {
      Displayer.NoEntriesFound();
      return;
    }

    CodingSession selection = ChoicesPrompt.SelectEntry(tableData, action, colorHighlight);

    AnsiConsole.MarkupLine($"[{colorHighlight}]Selection (id {selection.Id})[/]");
    Displayer.PreviewRow(selection);
    AnsiConsole.WriteLine();

    string[] choices = EnumExtensions.GetEditableColumn().ToArray();
    List<string> toEdit = ChoicesPrompt.SelectMultiString(action, colorHighlight, choices);

    if (toEdit.Contains($"{Column.activity.GetDisplayName()}"))
      selection.EditActivity(ChoicesPrompt.AskForActivity(action, colorHighlight));

    if (toEdit.Contains($"{Column.startTime.GetDisplayName()}") && toEdit.Contains($"{Column.endTime.GetDisplayName()}"))
    {
      string startDate = GetTimeFromUser(DaTeStyleShort, $"[{colorHighlight}]{action}[/] start date").ToString(DaTeStyleShort);
      string startHour = GetTimeFromUser(DateStyleHour, $"[{colorHighlight}]{action}[/] start hour").ToString(DateStyleHour);
      string endHour = GetTimeFromUser(DateStyleHour, $"[{colorHighlight}]{action}[/] end hour").ToString(DateStyleHour);
      timeInfos = FormatTimeInfo(startDate, startHour, endHour);

      selection.EditStartTime(timeInfos[0]);
      selection.EditEndTime(timeInfos[1]);
      selection.EditDuration(Convert.ToInt32(timeInfos[2]));
    }

    else if (toEdit.Contains($"{Column.startTime.GetDisplayName()}"))
    {
      string startDate = GetTimeFromUser(DaTeStyleShort, $"[{colorHighlight}]{action}[/] start date").ToString(DaTeStyleShort);
      string startHour = GetTimeFromUser(DateStyleHour, $"[{colorHighlight}]{action}[/] start hour").ToString(DateStyleHour);
      string endHour = selection.EndTime;
      timeInfos = FormatTimeInfo(startDate, startHour, endHour);

      selection.EditStartTime(timeInfos[0]);
      selection.EditDuration(Convert.ToInt32(timeInfos[2]));
    }

    else if (toEdit.Contains($"{Column.endTime.GetDisplayName()}"))
    {
      string startDate = TrimHour(selection.StartTime);
      string startHour = TrimDate(selection.StartTime);
      string endHour = GetTimeFromUser(DateStyleHour, $"[{colorHighlight}]{action}[/] end hour").ToString(DateStyleHour);
      timeInfos = FormatTimeInfo(startDate, startHour, endHour);

      selection.EditEndTime(timeInfos[1]);
      selection.EditDuration(Convert.ToInt32(timeInfos[2]));
    }

    Database.UpdateRow(selection);

    AnsiConsole.WriteLine();
    Displayer.PreviewRow(selection);
    AnsiConsole.WriteLine();
    AnsiConsole.MarkupLine($"[green]Entry succesfully [{colorHighlight}]edited.[/][/]");
    AnsiConsole.WriteLine();
  }

  internal static void DeleteItem()
  {
    Displayer.PrintTitle();

    if (Query.CountRows() == 0)
    {
      Displayer.NoEntriesFound();
      return;
    }

    string action = "Delete";
    string colorHighlight = "Red";

    List<CodingSession> tableData = ChoicesPrompt.AskForFilter(action, colorHighlight);
    AnsiConsole.WriteLine();

    if (tableData.Count == 0)
    {
      Displayer.NoEntriesFound();
      return;
    }

    List<CodingSession> selection = ChoicesPrompt.SelectMultiSessions(action, colorHighlight, tableData);
    Displayer.PreviewRows(selection);

    if (selection.Count == 0)
      AnsiConsole.MarkupLine($"[yellow]No entry has been deleted[/]");
    else
    {
      foreach (var entry in selection)
        Database.Delete(entry);

      if (selection.Count == 1)
        AnsiConsole.MarkupLine($"[green]{selection.Count} entry has been [red]deleted[/].[/]");

      else
        AnsiConsole.MarkupLine($"[green]{selection.Count} entries have been [red]deleted[/].[/]");
    }
    AnsiConsole.WriteLine();
  }
}