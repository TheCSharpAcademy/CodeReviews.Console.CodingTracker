namespace Patryk_MM.Console.CodingTracker.Models {
    public class CodingSession {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan Duration => EndDate - StartDate;

        public CodingSession(DateTime startDate, DateTime endTime) {
            StartDate = startDate;
            EndDate = endTime;
        }

        public CodingSession() {

        }

        public override string ToString() {
            return $"Session started: [cyan]{StartDate}[/] Session ended: [cyan]{EndDate}[/] Duration: [cyan]{Duration}[/]";
        }
    }
}
