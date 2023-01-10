namespace CodingTrackerConsole.Test;

[TestFixture]
public class ValidationTest
{
    private Validation validation = new();

    [Test]
    public void IsValidDate_Returns_True()
    {
        var date = "11-16-2022";

        var result = validation.IsValidDate(date);

        Assert.IsTrue(result);
    }

    [Test]
    public void IsValidDate_Returns_False()
    {
        var date = "16-11-2022";

        var result = validation.IsValidDate(date);

        Assert.IsFalse(result);
    }

    [Test]
    public void IsValidTime_Returns_True()
    {
        var time = "18:30";

        var result = validation.IsValidTime(time);

        Assert.IsTrue(result);
    }

    [Test]
    public void IsValidTime_Returns_False()
    {
        var time = "6:30";

        var result = validation.IsValidTime(time);

        Assert.IsFalse(result);
    }

    [TestCase("06")]
    [TestCase("26")]
    public void IsValidDay_Returns_True(string day)
    {
        var result = validation.IsValidDay(day);

        Assert.IsTrue(result);
    }

    [TestCase("5")]
    [TestCase("0")]
    [TestCase("93")]
    public void IsValidDay_Returns_False(string day)
    {
        var result = validation.IsValidDay(day);

        Assert.IsFalse(result);
    }

    [TestCase("06")]
    [TestCase("11")]
    public void IsValidMonth_Returns_True(string month)
    {
        var result = validation.IsValidMonth(month);

        Assert.IsTrue(result);
    }

    [TestCase("5")]
    [TestCase("0")]
    [TestCase("93")]
    [TestCase("993")]
    [TestCase("  ")]
    [TestCase("")]
    [TestCase("2333333333333333333333333333333")]
    public void IsValidMonth_Returns_False(string month)
    {
        var result = validation.IsValidMonth(month);

        Assert.IsFalse(result);
    }

    [TestCase("2006")]
    [TestCase("1999")]
    [TestCase("2022")]
    public void IsValidYear_Returns_True(string year)
    {
        var result = validation.IsValidYear(year);

        Assert.IsTrue(result);
    }

    [TestCase("55")]
    [TestCase("04")]
    [TestCase("0")]
    [TestCase("993")]
    [TestCase("  ")]
    [TestCase("")]
    [TestCase("2333333333333333333333333333333")]
    public void IsValidYear_Returns_False(string year)
    {
        var result = validation.IsValidYear(year);

        Assert.IsFalse(result);
    }
}
