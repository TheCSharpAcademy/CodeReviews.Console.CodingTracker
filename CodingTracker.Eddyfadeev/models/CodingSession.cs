namespace CodingTracker.models;

internal record CodingSession
{
    internal int Id { get; set; }
    internal DateTime StartTime { get; set; }
    internal DateTime EndTime { get; set; }
    internal TimeSpan Duration => EndTime - StartTime;
}