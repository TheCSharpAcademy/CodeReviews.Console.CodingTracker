namespace CodingTracker.Kakurokan
{
    internal class CodingSession
    {
        public int Id { get; set; }
        public DateTime Date { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public TimeSpan Duration { get { return EndTime - StartTime; } }

        public CodingSession(DateTime date, DateTime startTime, DateTime endTime)
        {
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
        }

        public CodingSession()
        {
        }

        public string ToStringWithoutId() => $@" Date: {Date:dd-MM-yyyy}
 Duration: {Duration}
 Start: {StartTime}
 End: {EndTime}";

        public string ToStringWithoutStartAndEnd() => $@" Id: {Id}
 Date: {Date:dd-MM-yyyy}
 Duration: {Duration}";

        public override string ToString() => $@" Id: {Id}
 Duration: {Duration}
 Date: {Date:dd-MM-yyyy}
 Start: {StartTime:HH:mm}
 End: {EndTime: HH:mm}";

    }
}

