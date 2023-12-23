using CodingTracker.FarSM.Models;
using ConsoleTableExt;
using System.Globalization;

namespace CodingTracker.FarSM
{
    internal class Helpers
    {
        public static void PrintData(List<CodingSession> table,string message)
        {
            ConsoleTableBuilder
                .From(table)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn(new List<string> { "ENTRY NO", "DATE", "START TIME", "END TIME", "DURATION" })
                .WithTitle(message.ToUpper(), ConsoleColor.DarkYellow, ConsoleColor.Black, TextAligntment.Center)
                .WithTextAlignment(new Dictionary<int, TextAligntment> {
                    { 2, TextAligntment.Center },{3, TextAligntment.Center },{4,TextAligntment.Center }
                 })
                 .WithMinLength(new Dictionary<int, int> {
                    { 1, 18 }, { 2, 15 }, {3,15}, {4,15}
                })
                .WithFormatter(1, f => $"{f:dd-MM-yyyy}")
                .WithFormatter(2, f => $"{f:hh:mm tt}")
                .WithFormatter(3, f => $"{f:hh:mm tt}")
                .WithFormatter(4, f => $"{f.ToString().Substring(0,5)}")
                .WithCharMapDefinition(
                    CharMapDefinition.FramePipDefinition,
                    new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.TopLeft, '╒' },
                        {HeaderCharMapPositions.TopCenter, '╤' },
                        {HeaderCharMapPositions.TopRight, '╕' },
                        {HeaderCharMapPositions.BottomLeft, '╞' },
                        {HeaderCharMapPositions.BottomCenter, '╪' },
                        {HeaderCharMapPositions.BottomRight, '╡' },
                        {HeaderCharMapPositions.BorderTop, '═' },
                        {HeaderCharMapPositions.BorderRight, '│' },
                        {HeaderCharMapPositions.BorderBottom, '═' },
                        {HeaderCharMapPositions.BorderLeft, '│' },
                        {HeaderCharMapPositions.Divider, '│' },
                    })
                .ExportAndWriteLine(TableAligntment.Center);

        }

        internal static TimeSpan CalculateDuration(DateTime time1, DateTime time2)
        {
            return time1 - time2;
        }

        internal static DateTime CompareDates(DateTime startTime, DateTime endTime)
        {
            DateTime StartTime = DateTime.Parse(startTime.ToString("HH:mm"));
            DateTime EndTime = DateTime.Parse(endTime.ToString("HH:mm"));

            DateTime clearTime = new DateTime();
            DateTime NewTime = new DateTime();
            DateTime validTime = endTime;
            string newTime = "";
            DateTime temp = new DateTime();

            while(EndTime <= StartTime)
            {
                Console.Write("\nEnd time should be after the start time. Please re-enter the time: ");
                newTime = Console.ReadLine();
                while (!DateTime.TryParseExact(newTime, "hh:mm tt", new CultureInfo("en-US"), DateTimeStyles.None, out validTime))
                {
                    Console.Write("Invalid input. Enter date in the format hh:mm am/pm : ");
                    newTime = Console.ReadLine();
                }
                EndTime = validTime;
            }

            return validTime;
        }

        internal static void PrintDuration(List<DateDuration> tableData, string message)
        {
            ConsoleTableBuilder
                .From(tableData)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn(new List<string> { "DATE","DURATION" })
                .WithTitle(message.ToUpper(), ConsoleColor.DarkYellow, ConsoleColor.Black, TextAligntment.Center)
                .WithTextAlignment(new Dictionary<int, TextAligntment> {
                    { 2, TextAligntment.Center },{3, TextAligntment.Center },{4,TextAligntment.Center }
                 })
                 .WithMinLength(new Dictionary<int, int> {
                    { 0, 18 }, { 1, 15 }
                })
                .WithFormatter(0, f => $"{f:dd-MM-yyyy}")
                .WithFormatter(1, f => $"{f.ToString().Substring(0, 5)}")
                .WithCharMapDefinition(
                    CharMapDefinition.FramePipDefinition,
                    new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.TopLeft, '╒' },
                        {HeaderCharMapPositions.TopCenter, '╤' },
                        {HeaderCharMapPositions.TopRight, '╕' },
                        {HeaderCharMapPositions.BottomLeft, '╞' },
                        {HeaderCharMapPositions.BottomCenter, '╪' },
                        {HeaderCharMapPositions.BottomRight, '╡' },
                        {HeaderCharMapPositions.BorderTop, '═' },
                        {HeaderCharMapPositions.BorderRight, '│' },
                        {HeaderCharMapPositions.BorderBottom, '═' },
                        {HeaderCharMapPositions.BorderLeft, '│' },
                        {HeaderCharMapPositions.Divider, '│' },
                    })
                .ExportAndWriteLine(TableAligntment.Left);

        }
    }
}
