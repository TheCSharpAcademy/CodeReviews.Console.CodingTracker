using CodingTracker.Ledana;
namespace CodingTrackerTests
{
    public class GetUserInputTests
    {

        [Test]
        public void GetDuration_WhenCalled_ReturnsDifferenceBetweenEndTimeAndStartTime()
        {
            GetUserInput getUserInput = new();
            Coding coding = new() { Start = "07:30", End = "14:30" };


            Assert.That(getUserInput.GetDuration(coding), Is.EqualTo("07:00:00"));
        }
    }
}
