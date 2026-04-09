using ConsoleTableExt;
using Spectre.Console;

namespace CodingTracker.Ledana
{
    internal class TableVisualisation
    {

        internal static void ShowTable(IEnumerable<Coding> tableData)
        {
            var table = new Table()
                .RoundedBorder()
                .BorderColor(Color.Red);
            table.AddColumn("Id");
            table.AddColumn("Date");
            table.AddColumn("Start");
            table.AddColumn("End");
            table.AddColumn("Duration");


            foreach (var item in tableData)
            {
                table.AddRow(item.Id.ToString(), item.Date, item.Start, item.End, item.Duration);
            }
            
            AnsiConsole.Write(table);
        }
    }
}