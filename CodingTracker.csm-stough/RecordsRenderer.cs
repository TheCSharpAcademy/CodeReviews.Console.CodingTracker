using ConsoleTableExt;

namespace CodeTracker.csm_stough
{
    public class RecordsRenderer : PaginatedTableRenderer
    {
        public RecordsRenderer(GetPage getPage, GetCount getCount, int limit = int.MaxValue, int offset = 0) : base(getPage, getCount, limit, offset) {
            lastPage = (int)Math.Max(Math.Ceiling(getCount() / (float)limit) - 1, 0);
        }

        public override void DisplayTable()
        {
            List<Object> sessions = getPage(resultsPerPage, resultsPerPage * currentPage);

            Console.Clear();

            ConsoleTableBuilder.From(TranslateData(sessions))
                .WithTitle($"All Records ~ Page {currentPage + 1} of {lastPage + 1}")
                .WithColumn("Start Date", "End Date", "Duration")
                .ExportAndWriteLine(TableAligntment.Left);

            base.DisplayTable();
        }

        protected override List<List<object>> TranslateData<T>(List<T> data)
        {
            List<List<Object>> tableData = new List<List<Object>>();

            for (int s = 0; s < data.Count; s++)
            {
                tableData.Add(new List<object>());
                tableData[s].Add((data[s] as CodingSession).startTime);
                tableData[s].Add((data[s] as CodingSession).endTime);
                tableData[s].Add((data[s] as CodingSession).duration.ToString("hh\\:mm"));
            }

            return tableData;
        }
    }
}
