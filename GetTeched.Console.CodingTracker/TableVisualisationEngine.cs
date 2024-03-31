using Microsoft.VisualBasic;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding_tracker;

public class TableVisualisationEngine
{
    internal void TableGeneration(List<string> columnHeaders, List<string> rowData)
    {
        var table = new Table()
            .Border(TableBorder.DoubleEdge);
        string[] columns = columnHeaders.ToArray();
        string[] rows = rowData.ToArray();

        for(int i=0; i<columns.Length; i++)
        {
            table.AddColumn($"[yellow]{columns[i]}[/]");
        }

        for(int i = 0; i < rows.Length; i += 4)
        {
            table.AddRow(rows[i], rows[i + 1], rows[i + 2], $"{rows[i + 3]}");
        }

        AnsiConsole.Write(table);
    }

    internal void StopwatchTable()
    {
        var table = new Table()
            .Border(TableBorder.DoubleEdge)
            .Caption("[red]Press F to stop[/]");
        table.AddColumn("[green]Stopwatch Elapsed Time[/]");
        table.AddColumn("             ");
        AnsiConsole.Write(table);
    }

    internal void WeekGenerator(List<string> rowData)
    {
        var table = new Table()
            .Border(TableBorder.DoubleEdge);
        string[] rows = rowData.ToArray();

        table.AddColumn("WeekNumber");
        for (int i = 0; i < rowData.Count - 1; i += 4)
        {
            table.AddRow(rowData[i]);
        }

        AnsiConsole.Write(table);
    }
}
