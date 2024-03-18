using System.Text;
using CodingTracker.models;
using Force.DeepCloner;
using Spectre.Console;

namespace CodingTracker.views;

/// <summary>
/// Represents a class for constructing summary tables for coding sessions.
/// </summary>
public class SummaryConstructor
{
    internal Table SummaryTable = new();
    internal Table SummaryTableForSaving = new();
    private TimeSpan _totalDuration;
    internal string FormattedDuration = "";

    /// <summary>
    /// Populates a table with coding session records.
    /// </summary>
    /// <param name="sessions">The list of coding sessions to populate the table with.</param>
    /// <param name="formatForSaving">Indicates whether the table should be formatted for saving or not. Defaults to false.</param>
    internal void PopulateWithRecords(IEnumerable<CodingSession> sessions, bool formatForSaving = false)
    {
        Table table = new()
        {
            Title = new TableTitle("Coding Sessions", new Style(Color.Grey100)),
            Border = TableBorder.Rounded,
            BorderStyle = new Style(Color.SpringGreen3)
        };

        table.AddColumn("Id");
        table.AddColumn("Start Date");
        table.AddColumn("End Date");
        table.AddColumn("Duration (hh:mm)");

        var tableForSaving = table.DeepClone();
        
        tableForSaving.Title = null;
        tableForSaving.BorderStyle = null;
        tableForSaving.HideFooters();
        tableForSaving.Columns[0].Width = 5;
        tableForSaving.Columns[1].Width = 24;
        tableForSaving.Columns[2].Width = 24;
        tableForSaving.Columns[3].Width = 16;
        
        int counter = 1;
        foreach (var session in sessions)
        {
            var color = counter % 5 == 0 ? "blue" : "grey100";

            if (counter == 5)
            {
                counter = 0;
            }

            table.AddRow(
                new Markup($"[{color}]{session.Id}[/]"),
                new Markup($"[{color}]{session.StartTime:dd-MM-yyyy HH:mm:ss}[/]"),
                new Markup($"[{color}]{session.EndTime:dd-MM-yyyy HH:mm:ss}[/]"),
                new Markup($"[{color}]{session.Duration.Hours:D2}:{session.Duration.Minutes:D2}[/]")
                );
            tableForSaving.AddRow(
                $"{session.Id}",
                $"{session.StartTime:dd-MM-yyyy HH:mm:ss}",
                $"{session.EndTime:dd-MM-yyyy HH:mm:ss}",
                $"{session.Duration.Hours:D2}:{session.Duration.Minutes:D2}"
                );
            counter++;

            _totalDuration += session.Duration;
        }

        FormatDuration();

        table.Caption = new TableTitle(FormattedDuration, new Style(Color.Green));
        
        SummaryTableForSaving = tableForSaving;
        SummaryTable = table;
    }

    /// <summary>
    /// Formats the total duration of coding sessions.
    /// </summary>
    private void FormatDuration()
    {
        var stringBuilder = new StringBuilder();
        
        string FormatUnit(string unit, int value) => value == 1 ? unit : unit + "s";

        stringBuilder
            .Append("Total Duration: ")
            .Append(
                (_totalDuration.Days > 0
                    ? _totalDuration.Days + " " + FormatUnit("day", _totalDuration.Days) + ", "
                    : "")
            )
            .Append(_totalDuration.Hours + " " + FormatUnit("hour", _totalDuration.Hours) + ", ")
            .Append(_totalDuration.Minutes + " " + FormatUnit("minute", _totalDuration.Minutes)
            );

        FormattedDuration = stringBuilder.ToString();
    }
}