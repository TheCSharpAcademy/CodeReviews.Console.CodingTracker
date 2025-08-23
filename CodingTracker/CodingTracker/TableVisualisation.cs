using System;
using System.Collections.Generic;
using ConsoleTableExt;

namespace coding_tracker
{
	internal class TableVisualisation
	{
		internal static void ShowTable<T>(List<T> tableData) where T : class
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