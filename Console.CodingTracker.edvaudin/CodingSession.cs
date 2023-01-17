using MapDataReader;

namespace CodingTracker;

[GenerateDataReaderMapper]
public class CodingSession
{
    public int Id { get; set; }
    public string Start_Time { get; set; } = string.Empty;
    public string End_Time { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"[#{Id}] Coded for {Duration} ({Start_Time} to {End_Time});";
    }
}
