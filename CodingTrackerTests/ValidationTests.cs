using CodingTracker;

namespace CodingTrackerTests;

public class ValidationTests
{
    [Test]
    public void DateToString_FormatsDateTime_AccordingToDateFormatConstant()
    {
        var testDate = new DateTime(2026, 10, 9, 14, 30, 0);
        string expectedOutput = "2026-10-09: 14-30";
        
        string result = Validation.DateToString(testDate);
        
        Assert.That(result, Is.EqualTo(expectedOutput));
    }
    
    [Test]
    public void DateToString_MatchesExpectedFormatPattern()
    {
        var testDate = DateTime.Now;
        
        string result = Validation.DateToString(testDate);
        
        Assert.That(result, Does.Match(@"^\d{4}-\d{2}-\d{2}: \d{2}-\d{2}$"));
    }
}