using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTracker.csm_stough
{
    public class RecordsRenderer : PaginatedTableRenderer
    {
        public RecordsRenderer(int limit = int.MaxValue, int offset = 0, string between = "", string low = "", string high = "") : 
            base(limit, offset, between, low, high)
        {
            lastPage = (int)Math.Max(Math.Ceiling(Database.GetCount(between, low, high) / (float)limit) - 1, 0);
        }

        public override void DisplayTable()
        {
            List<CodingSession> sessions = Database.GetAll(resultsPerPage, resultsPerPage * currentPage, between, low, high);

            Console.Clear();

            ConsoleTableBuilder.From(translateData(sessions))
                .WithTitle($"All Records ~ Page {currentPage + 1} of {lastPage + 1}")
                .WithColumn("Start Date", "End Date", "Duration")
                .ExportAndWriteLine(TableAligntment.Left);

            base.DisplayTable();
        }

        protected override List<List<object>> translateData<T>(List<T> data)
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
