namespace CodingTracker.Models;

public class CodingSessions
{
    public int Id { get; set; }
    private DateTime Date { get; set; }
    public string ShortDate { get; set; }
    public string TimeSpentCoding { get; set; }
}
