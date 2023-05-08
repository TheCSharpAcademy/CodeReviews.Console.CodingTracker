using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTableExt;

namespace CodeTracker.csm_stough
{
    public class Calender
    {

        public Calender()
        {
            Console.WriteLine("Logs This Month ~~~~~~~~~~~~~~~~~");
            ConsoleTableBuilder.From(CreateCurrentCalenderData())
                .WithTitle($"{Database.ExecuteString("select strftime('%Y-%m', 'now')")}")
                .WithColumn("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday")
                .ExportAndWriteLine(TableAligntment.Left);
        }

        public static List<List<Object>> CreateCurrentCalenderData()
        {
            int chunkSize = 7;
            int daysThisMonth = Database.ExecuteScalar("SELECT CAST(STRFTIME('%d', DATE('now', 'start of month','+1 month', '-1 day')) AS INTEGER)");
            int startingOffset = Database.ExecuteScalar("SELECT strftime('%w','now')");
            string text = "";
            List<Object> source = new List<Object>();

            for(int d = 0; d < startingOffset; d++)
            {
                source.Add("");
            }

            for(int d = 0; d < daysThisMonth; d++)
            {
                string duration = Database.FetchTotalDuration(d + 1) == TimeSpan.Zero ? "" : ": " + Database.FetchTotalDuration(d + 1).ToString();
                source.Add((d + 1).ToString() + duration);
            }

            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }



    }
}
