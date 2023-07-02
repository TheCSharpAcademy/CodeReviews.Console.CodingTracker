namespace CodingTracker.barakisbrown;

using ConsoleTableExt;

public class TableEngine
{
    public static void DisplayAllRecords(List<CodingSession> sessions)
    {
        ConsoleTableBuilder
            .From(sessions)
            .ExportAndWriteLine();
    }
}
