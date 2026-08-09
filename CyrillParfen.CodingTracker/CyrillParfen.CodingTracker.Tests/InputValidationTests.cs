using CyrillParfen.CodingTracker.Helpers;

namespace CyrillParfen.CodingTracker.Tests;

public class InputValidationTests
{
    [Theory]
    [InlineData("-1", false)]
    [InlineData("0", true)]
    [InlineData("1", true)]
    [InlineData("xyz", false)]
    public void IsValidInput_ReturnsPositiveIntegersAndZero(string input, bool expected)
    {
        bool result = InputValidation.IsInputValid(input, out _);
        
        Assert.Equal(expected, result);
    }
}
