using CodingTracker.kalsson;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodingTracker.Tests;

[TestClass]
public class ValidationTests
{
    /// <summary>
    /// Test if the ValidateDateTimeRange method returns true for valid date range.
    /// </summary>
    [TestMethod]
    public void WhenEndTimeIsLaterThanStartTime_ShouldReturnTrue()
    {
        // Arrange
        DateTime startTime = new DateTime(2023, 7, 1, 8, 0, 0);
        DateTime endTime = new DateTime(2023, 7, 1, 10, 0, 0);

        // Act
        bool result = Validation.ValidateDateTimeRange(startTime, endTime);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Test if the ValidateDateTimeRange method returns false when end time is before start time.
    /// </summary>
    [TestMethod]
    public void WhenEndTimeIsBeforeStartTime_ShouldReturnFalse()
    {
        // Arrange
        DateTime startTime = new DateTime(2023, 7, 1, 10, 0, 0);
        DateTime endTime = new DateTime(2023, 7, 1, 8, 0, 0);

        // Act
        bool result = Validation.ValidateDateTimeRange(startTime, endTime);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Test if the ValidateDateTimeRange method returns false when end time is equal to start time.
    /// </summary>
    [TestMethod]
    public void WhenEndTimeIsEqualToStartTime_ShouldReturnFalse()
    {
        // Arrange
        DateTime startTime = new DateTime(2023, 7, 1, 8, 0, 0);
        DateTime endTime = startTime;

        // Act
        bool result = Validation.ValidateDateTimeRange(startTime, endTime);

        // Assert
        Assert.IsFalse(result);
    }
}