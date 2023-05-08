using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTracker.csm_stough
{
    public class ReportRecord
    {
        public string Start;
        public int RecordsCount;
        public TimeSpan Duration;

        public ReportRecord(string Start, int RecordsCount, TimeSpan Duration)
        {
            this.Start = Start;
            this.RecordsCount = RecordsCount;
            this.Duration = Duration;
        }
    }
}
