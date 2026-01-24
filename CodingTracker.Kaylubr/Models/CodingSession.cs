namespace CodingTracker.Models;

internal class CodingSession
{
    internal int Id { get; set; }
    internal DateTime StartTime { get; set; }
    internal DateTime EndTime { get; set; }
    internal required string Duration { get; set; }
}