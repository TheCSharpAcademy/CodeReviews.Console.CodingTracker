using ConsoleTableExt;

namespace CodingTracker.TomDonegan
{
    internal class Helpers
    { 
        internal static String CalculateDuration(DateTime startTime, DateTime endTime)
        {
            if (endTime < startTime)
            {
                endTime = endTime.AddDays(1);
            }

            TimeSpan duration = endTime - startTime;  

            return duration.ToString();
        }

        internal static void TableBuilder(
            List<List<object>> tableData,
            string title,
            TextAligntment alignment,
            string tableType,
            List<string> columnHeaders = null
        )
        {
            if (tableType == "menuTable")
            {
                ConsoleTableBuilder
                    .From(tableData)
                    .WithTitle(title)
                    .WithTextAlignment(new Dictionary<int, TextAligntment> { { 0, alignment } })
                    .WithFormat(ConsoleTableBuilderFormat.Minimal)
                    .ExportAndWriteLine(TableAligntment.Center);
            }
            else
            {
                ConsoleTableBuilder
                    .From(tableData)
                    .WithTitle(title)
                    .WithColumn(columnHeaders)
                    .WithTextAlignment(
                        new Dictionary<int, TextAligntment>
                        {
                            { 0, alignment },
                            { 1, alignment },
                            { 2, alignment },
                            { 3, alignment },
                            { 4, alignment }
                        }
                    )
                    .WithMinLength(
                        new Dictionary<int, int>
                        {
                            { 1, 10 },
                            { 2, 10 },
                            { 3, 10 },
                            { 4, 10 },
                            { 5, 10 }
                        }
                    )
                    .WithFormat(ConsoleTableBuilderFormat.Alternative)
                    .ExportAndWriteLine(TableAligntment.Center);
            }
        }

        internal static void WaitForUserInput(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }

        internal static string GetUserInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
