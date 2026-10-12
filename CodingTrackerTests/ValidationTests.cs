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
    
        
    [TestCase("2026-10-12: 18-30", true)]
    [TestCase("  2026-10-12: 18-30  ", true)]
    public void DateTryParser_WithValidInput_ReturnsTrueAndParsedDate(string input, bool expectedResult)
    {
        bool success = Validation.DateTryParser(input, out DateTime parsedDate);
        
        Assert.That(success, Is.EqualTo(expectedResult));
        Assert.That(parsedDate, Is.Not.EqualTo(default(DateTime)));
    }
    
    [TestCase("12-10-2026")]
    [TestCase("not-a-date")]
    [TestCase("")]
    public void DateTryParser_WithInvalidInput_ReturnsFalse(string input)
    {
        bool success = Validation.DateTryParser(input, out DateTime parsedDate);
        
        Assert.That(success, Is.False);
    }
    
}