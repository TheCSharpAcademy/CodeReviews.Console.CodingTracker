using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTrackerConsoleUI
{

    public class TableVisualisation
    {
        public static void ShowTable<T>(List<T> tableData) where T :class
        {
            Console.WriteLine("\n\n");
            ConsoleTableBuilder
                .From(tableData)
                .WithTitle("Coding")
                .ExportAndWriteLine();
            Console.WriteLine("\n\n");
        }
    }
}