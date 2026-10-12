using CodingTracker;

namespace CodingTrackerTests;

public class CodingSessionTests
{
    [Test]
    public void DefaultConstructorCheck()
    {
        var session = new CodingSession();
        
        Assert.That(session.Id, Is.Null);
        Assert.That(session.StartTime, Is.EqualTo(default(DateTime)));
        Assert.That(session.EndTime, Is.Null);
        Assert.That(session.DurationTicks, Is.Null);
        Assert.That(session.Duration, Is.EqualTo(TimeSpan.Zero));
    }

    [Test]
    public void Duration_UpdatesDurationTicks()
    {
        var session = new CodingSession();
        var duration = TimeSpan.FromMinutes(45);
        session.Duration = duration;
        
        Assert.That(session.DurationTicks, Is.EqualTo(duration.Ticks));
        Assert.That(session.Duration, Is.EqualTo(duration));
    }

    [Test]
    public void Duration_ReturnsZero_WhenDurationTicksIsNull()
    {
        var session = new CodingSession
        {
            DurationTicks = null
        };
        
        Assert.That(session.Duration, Is.EqualTo(TimeSpan.Zero));
    }
}