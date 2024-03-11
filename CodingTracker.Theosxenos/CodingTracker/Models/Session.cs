namespace CodingTracker.Models;

public class Session
{
    public int Id { get; set; }
    public DateOnly Day { get; set; }

    public override string ToString()
    {
        return Day.ToString("d-M-yy");
    }
}