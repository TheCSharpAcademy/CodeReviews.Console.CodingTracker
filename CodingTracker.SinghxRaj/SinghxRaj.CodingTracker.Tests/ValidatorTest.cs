namespace SinghxRaj.CodingTracker.Tests;

[TestClass]
public class ValidatorTest
{
    [TestMethod]
    public void ValidOptionIsGiven()
    {
        // Arrange
        int option = 1;
        // Act
        bool validOption = Validator.ValidateOption(option);
        // Assert
        Assert.IsTrue(validOption);
    }

    [TestMethod]
    public void InvalidOptionIsGiven() 
    {
        // Arrange
        int option = 5;
        // Act
        bool validOption = Validator.ValidateOption(option);
        // Assert
        Assert.IsFalse(validOption);
    }

    [TestMethod]
    public void ValidateStartAndEndDateTimes() 
    {
        // Arrange
        DateTime start = DateTime.Now;
        DateTime end = DateTime.Now + TimeSpan.FromDays(1);
        // Act
        bool validStartAndEnd = Validator.ValidateSessionDateTimes(start, end);
        // Assert
        Assert.IsTrue(validStartAndEnd);
    }

    [TestMethod]
    public void InvalidStartAndEndDateTimes() 
    {
        // Arrange
        DateTime start = DateTime.Now;
        DateTime end = DateTime.Now - TimeSpan.FromDays(1);
        // Act
        bool validStartAndEnd = Validator.ValidateSessionDateTimes(start, end);
        // Assert
        Assert.IsFalse(validStartAndEnd);
    }

    [TestMethod]
    public void ValidDateString() 
    {
        // Arrange
        string dateStr = "10-09-23";
        // Act
        bool validDate = Validator.ValidateDate(dateStr);
        // Assert
        Assert.IsTrue(validDate);
    }

    [TestMethod]
    public void InvalidDateString() 
    {
        // Arrange
        string dateStr = "25-16-23";
        // Act
        bool validDate = Validator.ValidateDate(dateStr);
        // Assert
        Assert.IsFalse(validDate);
    }

    [TestMethod]
    public void ValidTimeString() 
    {
        // Arrange
        string timeStr = "10:45";
        // Act
        bool validTime = Validator.ValidateTime(timeStr);
        // Assert
        Assert.IsTrue(validTime);
    }

    [TestMethod]
    public void InvalidTimeString() 
    {
        // Arrange
        string timeStr = "96:05";
        // Act
        bool validTime = Validator.ValidateTime(timeStr);
        // Assert
        Assert.IsFalse(validTime);
    }
}