using CyrillParfen.CodingTracker.Helpers;
using System.Globalization;

namespace CyrillParfen.CodingTracker.Tests;

public class DateHelpersTests
{
    [Theory]
    [InlineData("30.01.2026 14:39", true)]
    [InlineData("30.01.26 14:39", false)]
    [InlineData("30.01.2026", false)]
    public void IsValidDate_ReturnsExpectedResult(string input, bool expected)
    {
        bool result = DateHelper.IsValidDate(input, "dd.MM.yyyy HH:mm", out _);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("30.01.2026 14:39", "30.01.2026 15:55", true)]
    [InlineData("30.01.2026 14:39", "30.01.2026 13:39", false)]
    [InlineData("30.01.2026 14:39", "29.01.2026 14:39", false)]
    public void IsStartBeforeEnd_ReturnsExpectedResult(string start, string end, bool expected)
    {
        const string dateFormat = "dd.MM.yyyy HH:mm";
        var culture = new CultureInfo("en-US");

        var startDate = DateTime.ParseExact(start, dateFormat, culture);
        var endDate = DateTime.ParseExact(end, dateFormat, culture);

        bool result = DateHelper.IsStartBeforeEnd(startDate, endDate);

        Assert.Equal(expected, result);
    }
}
