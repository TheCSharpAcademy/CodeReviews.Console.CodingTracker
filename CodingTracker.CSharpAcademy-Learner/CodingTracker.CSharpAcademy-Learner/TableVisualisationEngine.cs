using CodingTracker.CSharpAcademy_Learner.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.CSharpAcademy_Learner
{
    internal static class TableVisualisationEngine
    {
        internal static void DisplaySessions(List<CodingSession> sessions)
        {
            var table = new Table();
            table.Border(TableBorder.Rounded);

            table.AddColumn("ID");
            table.AddColumn("Start Date");
            table.AddColumn("End Date");
            table.AddColumn("Duration");

            foreach(var session in sessions)
            {
                table.AddRow(
                    session.Id.ToString(),
                    session.StartTime,
                    session.EndTime,
                    Validation.ShowDurationInFriendlyFormat(session.Duration)
                    );
            }

            AnsiConsole.Write(table);
        }
    }
}
