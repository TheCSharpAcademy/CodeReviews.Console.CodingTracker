using ConsoleTableExt;

namespace CodeTracker;

internal class TableLayout
{
    public static void DisplayTable(List<CodingSession> sessions)
    {
        var tableData = new List<List<Object>>();
        foreach (CodingSession codingSession in sessions)
        {
            tableData.Add(new List<Object>
            {
                codingSession.Id,
                codingSession.Date,
                codingSession.TimeStart,
                codingSession.TimeEnd,
                codingSession.TimeSpan
            });
        }
        ConsoleTableBuilder.From(tableData).WithColumn("Id", "Date", "Time Start", "Time End", "Time Spent Coding").ExportAndWriteLine();
    }
}
