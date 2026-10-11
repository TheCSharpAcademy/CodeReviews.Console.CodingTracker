using Spectre.Console;
using static CodingTracker.Enums;
using static CodingTracker.TimeFormatter;
using System.Linq;

namespace CodingTracker;

internal class Displayer
{
  internal static void PreviewRow(string activity, string startTime, string endTime, int duration)
  {
    var table = new Table()
            .RoundedBorder();

    table.AddColumn(Markup.Escape(Enums.Column.activity.GetDisplayName()));
    table.AddColumn(Markup.Escape(Enums.Column.startTime.GetDisplayName()));
    table.AddColumn(Markup.Escape(Enums.Column.endTime.GetDisplayName()));
    table.AddColumn(Markup.Escape(Enums.Column.duration.GetDisplayName()));

    table.AddRow(
      Markup.Escape(activity),
      Markup.Escape(ConvertDateOutDb(startTime)),
      Markup.Escape(ConvertHourOutDb(endTime)),
      Markup.Escape(IntToHHmm(duration)));

    AnsiConsole.Write(table);
  }

  internal static void PreviewRow(CodingSession session)
  {
    var table = new Table()
            .RoundedBorder();


    table.AddColumn(Markup.Escape(Enums.Column.activity.GetDisplayName()));
    table.AddColumn(Markup.Escape(Enums.Column.startTime.GetDisplayName()));
    table.AddColumn(Markup.Escape(Enums.Column.endTime.GetDisplayName()));
    table.AddColumn(Markup.Escape(Enums.Column.duration.GetDisplayName()));

    table.AddRow(
      Markup.Escape(session.Activity),
      Markup.Escape(ConvertDateOutDb(session.StartTime)),
      Markup.Escape(ConvertHourOutDb(session.EndTime)),
      Markup.Escape(IntToHHmm(session.Duration)));

    AnsiConsole.Write(table);
  }

  internal static void PreviewRows(List<CodingSession> sessions)
  {
    var table = new Table()
            .RoundedBorder();

    table.AddColumn(Markup.Escape(Enums.Column.activity.GetDisplayName()));
    table.AddColumn(Markup.Escape(Enums.Column.startTime.GetDisplayName()));
    table.AddColumn(Markup.Escape(Enums.Column.endTime.GetDisplayName()));
    table.AddColumn(Markup.Escape(Enums.Column.duration.GetDisplayName()));

    foreach (var session in sessions)
    {
      table.AddRow(
          Markup.Escape(session.Activity),
          Markup.Escape(ConvertDateOutDb(session.StartTime)),
          Markup.Escape(ConvertHourOutDb(session.EndTime)),
          Markup.Escape(IntToHHmm(session.Duration)));
    }

    AnsiConsole.Write(table);
  }

  internal static void PrintTable(List<CodingSession> tableData, string title)
  {
    var table = new Table()
            .RoundedBorder()
            .Title(title);

    foreach (string column in Enum.GetNames<Column>())
      table.AddColumn(column);

    foreach (var entry in tableData)
      table.AddRow(
        Markup.Escape(entry.Id.ToString()),
        Markup.Escape(entry.Activity),
        Markup.Escape(ConvertDateOutDb(entry.StartTime)),
        Markup.Escape(ConvertHourOutDb(entry.EndTime)),
        Markup.Escape(IntToHHmm(entry.Duration)));

    AnsiConsole.Write(table);
  }

  internal static void PrintTitle()
  {
    Console.Clear();
    AnsiConsole.Write(new Panel("   Coding Tracker   ")
      .AsciiBorder());
    AnsiConsole.WriteLine();
  }

  internal static void NoEntriesFound()
  {
    AnsiConsole.MarkupLine($"[red]No entries have been found.[/]");
    AnsiConsole.WriteLine();
  }
}