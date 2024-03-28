using ConsoleTableExt;
using Spectre.Console;
using System;
using System.Collections.Generic;

namespace CodingTracker
{
    public static class TableVisualisation
    {
        internal static void ShowTable<T>(List<T> tableData) where T : class
        {
            DisplayData(tableData);
        }

        internal static void DisplayReport(List<Coding> reportData, TimeSpan? totalTime = null)
        {
            DisplayData(reportData);
            if (totalTime != null)
            {
                AnsiConsole.MarkupLine($"Total Time: {totalTime}");
            }
        }

        private static void DisplayData<T>(List<T> data) where T : class
        {
            var table = new Table();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                table.AddColumn(new TableColumn(property.Name).Centered());
            }

            foreach (var item in data)
            {
                var row = new List<string>();
                foreach (var property in properties)
                {
                    var value = property.GetValue(item);
                    row.Add(value?.ToString() ?? string.Empty);
                }
                table.AddRow(row.ToArray());
            }

            AnsiConsole.Write(table);
        }
    }
}
