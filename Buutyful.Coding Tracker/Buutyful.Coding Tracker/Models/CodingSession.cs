namespace Buutyful.Coding_Tracker.Models;

public record CodingSession
{
    public Guid Id { get; }
    public DateTime StartAt { get; }
    public DateTime EndAt { get; }
    public TimeSpan Duration { get; }
    private CodingSession(Guid id, DateTime start, DateTime end, TimeSpan duration) =>
        (Id, StartAt, EndAt, Duration) = (id, start, end, duration);
    public static CodingSession Create(DateTime start, DateTime end)
    {
        var duration = end - start;
        return new CodingSession(Guid.NewGuid(), start, end, duration);
    }
    public static CodingSession Map(Guid id,DateTime start, DateTime end, TimeSpan duration) =>
        new(id, start, end, duration);

}
