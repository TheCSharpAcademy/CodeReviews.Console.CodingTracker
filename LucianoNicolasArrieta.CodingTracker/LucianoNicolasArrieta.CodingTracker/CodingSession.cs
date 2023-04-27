namespace coding_tracker
{
    public class CodingSession
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }

        public CodingSession() { }
        public CodingSession(string startTime, string endTime)
        {
            StartTime = DateTime.ParseExact(startTime, "d/M/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            EndTime = DateTime.ParseExact(endTime, "d/M/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            Duration = EndTime - StartTime;
        }

        internal CodingSession TrackCurrentSession()
        {
            string start = DateTime.Now.ToString("d/M/yyyy HH:mm");
            string input = "";
            while (input != "f")
            {
                Console.WriteLine("Type 'f' to finish the session");
                input = Console.ReadLine();
            }
            string end = DateTime.Now.ToString("d/M/yyyy HH:mm");

            this.StartTime = DateTime.ParseExact(start, "d/M/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            this.EndTime = DateTime.ParseExact(end, "d/M/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            this.Duration = EndTime - StartTime;

            return this;
        }
    }
}