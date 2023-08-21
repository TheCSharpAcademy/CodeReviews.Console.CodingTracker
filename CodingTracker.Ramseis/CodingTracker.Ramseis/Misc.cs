using ConsoleTableExt;

namespace CodingTracker.Ramseis
{
    internal class Misc
    {
        public static int GetIntegerInput()
        {
            int.TryParse(Console.ReadLine().Trim(), out int input);
            return input;
        }
        public static void PrintTable(List<List<object>> data)
        {
            ConsoleTableBuilder
                .From(data)
                .WithTitle("Tracked Coding Sessions")
                .WithColumn("ID", "Start", "End", "Duration")
                .WithTextAlignment(new Dictionary<int, TextAligntment>
                {
                    {0, TextAligntment.Center },
                    {1, TextAligntment.Center },
                    {2, TextAligntment.Center },
                    {3, TextAligntment.Right }
                })
                .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
                .WithCharMapDefinition(
                    CharMapDefinition.FramePipDefinition,
                    new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.TopLeft, '╒' },
                        {HeaderCharMapPositions.TopCenter, '═' },
                        {HeaderCharMapPositions.TopRight, '╕' },
                        {HeaderCharMapPositions.BottomLeft, '╞' },
                        {HeaderCharMapPositions.BottomCenter, '╤' },
                        {HeaderCharMapPositions.BottomRight, '╡' },
                        {HeaderCharMapPositions.BorderTop, '═' },
                        {HeaderCharMapPositions.BorderRight, '│' },
                        {HeaderCharMapPositions.BorderBottom, '═' },
                        {HeaderCharMapPositions.BorderLeft, '│' },
                        {HeaderCharMapPositions.Divider, ' ' },
                    })
                .ExportAndWriteLine();
        }
    }
}
