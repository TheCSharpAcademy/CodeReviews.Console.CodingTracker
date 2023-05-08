using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTracker.csm_stough
{
    internal class ReportsRenderer : PaginatedTableRenderer
    {
        private string timeFormat;

        public ReportsRenderer(string timeFormat, int limit = int.MaxValue, int offset = 0, string between = "", string low = "", string high = "") :
            base(limit, offset, between, low, high)
        {
            this.timeFormat = timeFormat;
            lastPage = (int)Math.Ceiling(Database.GetCountGroupedByTime(timeFormat) / (float)limit) - 1;
        }

        public override void DisplayTable()
        {
            List<ReportRecord> reports = Database.GetAllGroupedByTime(timeFormat, resultsPerPage, resultsPerPage * currentPage);

            Console.Clear();

            ConsoleTableBuilder.From(translateData(reports))
                .WithTitle($"All Records ~ Page {currentPage + 1} of {lastPage + 1}")
                .WithColumn("Date Logged", "Number Of Records", "Total Duration")
                .ExportAndWriteLine(TableAligntment.Left);

            Console.WriteLine($"\nAverage duration for this period: {AverageDuration(reports).ToString("hh\\:mm")} hours\n");

            base.DisplayTable();
        }

        protected override List<List<object>> translateData<T>(List<T> data)
        {
            List<List<Object>> tableData = new List<List<Object>>();

            for (int s = 0; s < data.Count; s++)
            {
                tableData.Add(new List<object>());
                tableData[s].Add((data[s] as ReportRecord).Start);
                tableData[s].Add((data[s] as ReportRecord).RecordsCount);
                tableData[s].Add((data[s] as ReportRecord).Duration);
            }

            return tableData;
        }

        private TimeSpan AverageDuration(List<ReportRecord> reports)
        {
            return TimeSpan.FromSeconds(reports.Select(s => s.Duration.TotalSeconds).Average());
        }
    }
}
