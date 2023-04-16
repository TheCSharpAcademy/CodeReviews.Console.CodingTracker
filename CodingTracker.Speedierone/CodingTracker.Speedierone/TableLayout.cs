using ConsoleTableExt;
using Microsoft.VisualBasic;


namespace CodeTracker
{
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
                    codingSession.TimeSpan
                });
            }
            ConsoleTableBuilder.From(tableData).WithColumn("Id", "Date", "Time Spent Coding").ExportAndWriteLine();
        }
    }
}
