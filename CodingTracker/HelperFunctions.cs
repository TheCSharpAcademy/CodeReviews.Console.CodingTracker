using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    internal class HelperFunctions
    { 
        internal static void ContinueToMainMenu()
        {
            AnsiConsole.Write(new Markup("[italic yellow]\nPress any key to continue to Main Menu[/]"));
            Console.ReadKey();
            Console.Clear();
            UserInterface.MainMenu();
        }
        internal static TimeSpan CalculateDuration(string StartTime, string EndTime)
        {
            CultureInfo provider = new CultureInfo("en-150");

            DateTime startTime = DateTime.ParseExact(StartTime, "yyyy-MM-dd HH:mm:ss", provider);
            DateTime endTime = DateTime.ParseExact(EndTime, "yyyy-MM-dd HH:mm:ss", provider);

            TimeSpan duration = endTime - startTime;
            return duration;
        }
        internal static Table CreateTable()
        {
            var table = new Table();
            table.AddColumn(new TableColumn("Id").Centered());
            table.AddColumn(new TableColumn("Duration").Centered());
            table.AddColumn(new TableColumn("Start Time").Centered());
            table.AddColumn(new TableColumn("End Time").Centered());
            table.Expand();
            return table;
        }
    }
}
