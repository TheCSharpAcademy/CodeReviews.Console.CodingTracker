namespace CodingTrackerTests;
using CodingTracker;

public class Tests
{
    private AppSettings validAppSettings;
    
    [SetUp]
    public void Setup()
    {
        validAppSettings = AppSettings.LoadConfig();
    }

    [Test]
    public void Test1()
    {
        string expectedDbPath = "CodingTracker.db";
        Assert.That(validAppSettings.DbPath, Is.EqualTo(expectedDbPath));
    }
}