using CodingTracker.K_MYR.Models;
using ConsoleTableExt;

namespace CodingTracker.K_MYR
{
    internal class Helpers
    {
        internal static void PrintAllRecords()
        {
            Console.Clear();
            var records = SQLiteOperations.SelectAllRecords();
            PrintRecords(records);                             
        }

        internal static void PrintRecords(List<CodingSession> tableData, bool reverse = false)
        {
            Console.Clear();      

            if (tableData.Count > 0)
            {
                tableData.Sort((x, y) => DateTime.Compare(x.StartTime, y.StartTime));

                if (reverse)
                    tableData.Reverse();

                ConsoleTableBuilder
                    .From(tableData)
                    .WithTitle("Coding Sessions", ConsoleColor.Green, ConsoleColor.Black)
                    .WithTextAlignment(new Dictionary<int, TextAligntment>
                        {
                        {2, TextAligntment.Center },
                        {1, TextAligntment.Center }
                        })
                    .ExportAndWriteLine();
            }
            else
            {
                Console.WriteLine("Now records were found!");
            }
        }        

        internal static List<CodingSession> GetRecords(string unit, int timespanNumber)
        {
            var records = SQLiteOperations.SelectAllRecords();
            TimeSpan period = new();

            switch (unit)
            {
                case "d":
                    period = TimeSpan.FromDays(timespanNumber);
                    break;
                case "w":
                    period = TimeSpan.FromDays(timespanNumber * 7);
                    break;
                case "y":
                    period = TimeSpan.FromDays(timespanNumber * 365);
                    break;
            }

            var tableData = records.Where(x => x.StartTime >= DateTime.Now.Date.Subtract(period)).ToList();

            return tableData;
        }

        internal static void PrintStopwatchMenu(string startDate = "", string endDate = "", string duration = "")
        {
            Console.Clear();
            Console.WriteLine("| Track your coding session via stopwatch |");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine($"Start Time: {startDate}");
            Console.WriteLine($"End Time: {endDate}");
            Console.WriteLine($"Duration: {duration}");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("0 - Start/Stop the timer");
            Console.WriteLine("1 - Save Session");
            Console.WriteLine("2 - Return to main menu");
            Console.WriteLine("-------------------------------------------");
        }
    }
}
