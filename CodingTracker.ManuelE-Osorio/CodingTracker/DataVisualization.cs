using ConsoleTableExt;

namespace CodingTracker;

class DataVisualization
{
    public static void PrintTable(List<CodingSession> sessions)
    {  
        var tableData = new List<List<object>>();
        tableData.Clear();
        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();
        foreach (CodingSession session in sessions)
        {
            tableData.Add(new List<object>{session.ID,session.StartDateTime.ToString("yyyy/MM/dd HH:mm"),
            session.EndDateTime.ToString("yyyy/MM/dd HH:mm"),
            session.ElapsedTime.ToString("d\\.hh\\:mm")});
        }

        ConsoleTableBuilder
        .From(tableData)
        .WithTitle("Coding Sessions")
        .WithColumn("ID", "Start Time", "End Time","Session Lenght")
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
}   