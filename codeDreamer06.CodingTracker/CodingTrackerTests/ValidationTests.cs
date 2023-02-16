using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodingTracker;

namespace CodingTrackerTests;

[TestClass]
public class ValidationTests
{
    [TestMethod]
    public void GetNumber_InputIsNotNumber_ReturnsZero()
    {
        int result = "add two".GetNumber("add");
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void RemoveKeyword_EmptyString_ReturnsEmptyString()
    {
        var result = string.Empty.RemoveKeyword("add");
        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void RemoveKeyword_EmptyKeyword_ReturnsEmptyString()
    {
        var result = "add".RemoveKeyword(string.Empty);
        Assert.AreEqual("add", result);
    }

    [TestMethod]
    public void SplitTime_EmptyString_ReturnsNull()
    {
        var result = string.Empty.SplitTime(string.Empty);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void SplitTime_OnlyHours_ReturnsTimeSpan()
    {
        var result = "add 5".SplitTime("add");
        Assert.AreEqual(5, result!.Value.TotalHours);
    }

    [TestMethod]
    public void SplitTime_HoursAndMinutes_ReturnsTimeSpan()
    {
        var result = "add 5:30".SplitTime("add");
        Assert.AreEqual(new TimeSpan(5, 30, 0), result);
    }

    [TestMethod]
    public void IsInvalidForUpdate_LengthIsThree_ReturnsFalse()
    {
        var result = "update 2 6".IsInvalidForUpdate();
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsInvalidForUpdate_LengthIsTwo_ReturnsTrue()
    {
        var result = "update 2".IsInvalidForUpdate();
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsDurationValid_TwentySixHours_ReturnsFalse()
    {
        var result = new TimeSpan(26, 0, 0).IsDurationValid();
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsDurationValid_SixHours_ReturnsTrue()
    {
        var result = new TimeSpan(6, 0, 0).IsDurationValid();
        Assert.IsTrue(result);
    }
}