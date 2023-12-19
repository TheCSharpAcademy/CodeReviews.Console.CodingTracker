using System.ComponentModel.DataAnnotations;

namespace CodingTracker.iGoodw1n;

public class CodingSession
{
    public int Id { get; set; }

    [Required]
    public string Language { get; set; } = null!;
    public DateTime Start { get; set; }

    [IsDateAfter(nameof(Start))]
    public DateTime End { get; set; }

    public TimeSpan Duration => End - Start;

    public override string ToString()
    {
        return $"Record #{Id}. Language: {Language}, start time: {Start}, end time: {End}, duration: {Duration}";
    }
}