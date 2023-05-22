namespace CodeTracker.csm_stough
{
    public class ReportRecord
    {
        public string Start;
        public int RecordsCount;
        public TimeSpan Duration;
        public TimeSpan AverageHours;

        public ReportRecord(string Start, int RecordsCount, TimeSpan Duration)
        {
            this.Start = Start;
            this.RecordsCount = RecordsCount;
            this.Duration = Duration;
            AverageHours = AverageDuration(Duration, RecordsCount);
        }

        private TimeSpan AverageDuration(TimeSpan totalTime, int reportsCount)
        {
            return TimeSpan.FromSeconds(totalTime.TotalSeconds / reportsCount);
        }
    }
}
