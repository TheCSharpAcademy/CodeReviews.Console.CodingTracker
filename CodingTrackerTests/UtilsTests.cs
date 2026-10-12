using CodingTracker;

namespace CodingTrackerTests;

public class UtilsTests
{
    [Test]
    public void Duration_WithValidDates()
    {
        var start = new DateTime(2026, 10, 12, 10, 0, 0);
        var end = new DateTime(2026, 10, 12, 12, 30, 0);
        TimeSpan result = Utils.Duration(start, end);
        
        Assert.That(result, Is.EqualTo(TimeSpan.FromHours(2.5)));
    }

    [Test]
    public void Duration_ThrowsException()
    {
        var start = new DateTime(2026, 10, 12, 14, 0, 0);
        var end = new DateTime(2026, 10, 12, 12, 0, 0);
        
        var ex = Assert.Throws<Exception>(() => Utils.Duration(start, end));
        Assert.That(ex.Message, Is.EqualTo("Start must be earlier than end"));
    }

    [Test]
    public void GetFirstDayMonth()
    {
        var date = new DateTime(2026, 10, 12, 15, 45, 30);
        DateTime result = Utils.GetFirstDayMonth(date);

        var expected = new DateTime(2026, 10, 1, 0, 0, 0);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void PreviousMonth()
    {
        var date = new DateTime(2026, 10, 12);
        DateTime result = Utils.PreviousMonth(date);
        
        var expected = new DateTime(2026, 9, 12);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void NextMonth()
    {
        var date = new DateTime(2026, 10, 12);
        DateTime result = Utils.NextMonth(date);
        
        var expected = new DateTime(2026, 11, 12);
        Assert.That(result, Is.EqualTo(expected));
    }
}