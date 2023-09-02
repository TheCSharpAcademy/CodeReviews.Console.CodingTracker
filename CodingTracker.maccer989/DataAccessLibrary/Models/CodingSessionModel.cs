namespace DataAccessLibrary.Models
{

    public class CodingSessionModel
    {
        public int Id { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int DurationInMinutes { get; set; }
    }
}