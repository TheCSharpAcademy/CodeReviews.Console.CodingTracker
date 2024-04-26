namespace coding_tracker;

internal class CodingSession
{
    public int Id { get; set; }
    public string Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Duration { get; set; }
}