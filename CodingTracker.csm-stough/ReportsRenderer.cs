using ConsoleUtilities;

namespace CodeTracker.csm_stough
{
    internal class ReportsRenderer : PaginatedTableRenderer
    {
        private string timeFormat;
        private string unit;

        public ReportsRenderer(string timeFormat, string unit, GetPage getPage, GetCount getCount, int limit = int.MaxValue, int offset = 0) : base(getPage, getCount, limit, offset)
        {
            this.timeFormat = timeFormat;
            this.unit = unit;
            lastPage = (int)Math.Max(Math.Ceiling(SessionDao.GetCountGroupedByTime(timeFormat) / (float)limit) - 1, 0);
        }

        public override void DisplayTable()
        {
            List<ReportRecord> reports = SessionDao.GetAllGroupedByTime(timeFormat, resultsPerPage, resultsPerPage * currentPage);

            Console.Clear();

            Menu recordsMenu = new Menu($"Logs Filtered By {unit} ~ Page {currentPage + 1} of {lastPage + 1}");

            if (currentPage != 0)
            {
                recordsMenu.AddOption("F", "First Page...", () => { currentPage = 0; DisplayTable(); });
                recordsMenu.AddOption("P", "Previous Page...", () => { currentPage--; DisplayTable(); });
            }

            int num = 1;
            reports.ForEach(report =>
            {
                recordsMenu.AddOption(num.ToString(), string.Format($"{report.Start} : {report.RecordsCount} records : {report.Duration} hours : {report.AverageHours.ToString("hh\\:mm")} hours/{unit}"), () => {
                    RecordsRenderer recordsRenderer = new RecordsRenderer((limit, offset) =>
                    {
                        return new List<Object>(SessionDao.GetAll(limit, offset, $"STRFTIME('{timeFormat}', Start) = '{report.Start}'"));
                    },
                    () =>
                    {
                        return SessionDao.GetCount($"STRFTIME('{timeFormat}', Start) = '{report.Start}'");
                    },
                    resultsPerPage);
                    recordsRenderer.DisplayTable();
                });
                num++;
            });

            if (currentPage != lastPage)
            {
                recordsMenu.AddOption("N", "Next Page...", () => { currentPage++; DisplayTable(); });
                recordsMenu.AddOption("L", "Last Page...", () => { currentPage = lastPage; DisplayTable(); });
            }

            recordsMenu.AddOption("B", "Go Back...", () => { });

            recordsMenu.SelectOption(false);
        }

        protected override List<List<object>> TranslateData<T>(List<T> data)
        {
            List<List<Object>> tableData = new List<List<Object>>();

            for (int s = 0; s < data.Count; s++)
            {
                tableData.Add(new List<object>());
                tableData[s].Add((data[s] as ReportRecord).Start);
                tableData[s].Add((data[s] as ReportRecord).RecordsCount);
                tableData[s].Add((data[s] as ReportRecord).Duration);
                tableData[s].Add((data[s] as ReportRecord).AverageHours.ToString("hh\\:mm"));
            }

            return tableData;
        }
    }
}
