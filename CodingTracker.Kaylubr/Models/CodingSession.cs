namespace CodingTracker.Models;

internal class CodingSession
{
    int Id { get; set; }
    DateTime StartTime { get; set; }
    DateTime EndTime { get; set; }
    TimeSpan duration { get; set; }
}