namespace CodingTracker;

public class CodingSession
{
    private int id;
    private DateTime date;
    private DateTime startAt;
    private DateTime endAt;
    private readonly TimeSpan duration;

    public int Id { get => id; set => id = value; }
    public DateTime Date { get => date.Date; set => date = value; }
    public DateTime StartAt { get => startAt; set => startAt = value; }
    public DateTime EndAt { get => endAt; set => endAt = value; }
    public TimeSpan Duration { get => duration; } // Duration will just readonly it calculated automatically

    public CodingSession(int id, DateTime date, DateTime startAt, DateTime endAt)
    {
        this.id = id;
        this.date = date;
        this.startAt = startAt;
        this.endAt = endAt;
        this.duration = this.CalculateDuration();
    }

    private TimeSpan CalculateDuration()
    {
        return this.endAt.Subtract(this.startAt);
    }

    public override string ToString()
    {
        return $@"{this.Id} - {this.Date} => from {this.StartAt} to {this.EndAt} | Last : {this.Duration}";
    }
}
