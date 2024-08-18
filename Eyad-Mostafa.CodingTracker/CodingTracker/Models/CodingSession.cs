namespace CodingTracker.Models;

internal class CodingSession
{
    public int Id { get; set; }
    public string Date { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public int Duration {  get; set; }
    public string DurationFormatted { get; set; } = string.Empty;
    public string Description {  get; set; } = string.Empty;

}
