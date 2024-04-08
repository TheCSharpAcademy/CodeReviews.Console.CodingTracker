namespace CodingTracker.Models;

public class CodingSession
{
    public CodingSession(string id, string start, string end)
    {
        Id = id;
        StartTime = start;
        EndTime = end;
    }
    
    public string Id { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
}