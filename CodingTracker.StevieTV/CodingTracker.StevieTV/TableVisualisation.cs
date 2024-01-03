using ConsoleTableExt;

namespace CodingTracker.StevieTV;

internal class TableVisualisation
{
    internal static void ShowTable<T>(List<T> tableData) where T : class
    {
        ConsoleTableBuilder
            .From(tableData)
            .WithTitle("Coding Sessions", ConsoleColor.Yellow, ConsoleColor.DarkGray)
            .WithColumn("ID", "Date", "Start Time", "End Time", "Duration")
            .WithTextAlignment(new Dictionary<int, TextAligntment>
            {
                {2, TextAligntment.Right},
                {3, TextAligntment.Right},
                {4, TextAligntment.Right},
            })
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
            .ExportAndWriteLine();
        Console.WriteLine("\n");
    }
}