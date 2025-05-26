namespace CodingTracker.Golvi1124.Models;

internal class CodingRecord
{
    internal int Id { get; set; }
    internal DateTime DateStart { get; set; }
    internal DateTime DateEnd { get; set; }
    internal TimeSpan Duration { get; set; }
}