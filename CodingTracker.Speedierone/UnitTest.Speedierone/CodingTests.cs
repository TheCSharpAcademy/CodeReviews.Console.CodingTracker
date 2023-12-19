using CodeTracker;

namespace UnitTest.Speedierone
{
    [TestClass]
    public class CodingTests
    {
        [TestMethod]
        public void GetDate_CorrectFormat_ReturnsDate()
        {
            string input = "01-01-23";
            StringReader stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            string result = Helpers.GetDate();

            Assert.AreEqual(input, result);
        }
        [TestMethod]
        public void GetDate_InvalidInputThenValidInput_ReturnsValidDate()
        {
            string invalidInput = "invalid";
            string validInput = "01-01-23";
            StringReader stringReader = new StringReader(invalidInput + Environment.NewLine + validInput);
            Console.SetIn(stringReader);

            string result = Helpers.GetDate();

            Assert.AreEqual(validInput, result);
        }
        [TestMethod]
        public void GetStartTime_CorrectFormat_ReturnsStartTime()
        {
            string input = "23:00";
            StringReader stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            string result = Helpers.GetStartTime();

            Assert.AreEqual(input, result);
        }
        [TestMethod]
        public void GetStartTime_InvalidInputThenValid_ReturnsValidStartTime()
        {
            string invalidInput = "invalid";
            string validInput = "23:00";
            StringReader stringReader = new StringReader(invalidInput + Environment.NewLine + validInput);
            Console.SetIn(stringReader);

            string result = Helpers.GetStartTime();

            Assert.AreEqual(validInput, result);
        }
        [TestMethod]
        public void GetEndTime_CorrectFormat_ReturnsEndTime()
        {
            string input = "23:00";
            StringReader stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            string result = Helpers.GetEndTime();

            Assert.AreEqual(input, result);
        }
        [TestMethod]
        public void GetEndTime_InvalidInputThenValid_ReturnsValidEndTime()
        {
            string invalidInput = "invalid";
            string validInput = "23:00";
            StringReader stringReader = new StringReader(invalidInput + Environment.NewLine + validInput);
            Console.SetIn(stringReader);

            string result = Helpers.GetEndTime();

            Assert.AreEqual(validInput, result);
        }
        [TestMethod]
        public void CodingTime_ReturnsCorrectCodingTimeDifference()
        {
            string timeStart = "2023-01-01T12:00:00";
            string timeEnd = "2023-01-01T13:00:00";

            string result = Helpers.CodingTime(timeStart, timeEnd);

            Assert.AreEqual("01:00:00", result);
        }
        [TestMethod]
        public void CodingTime_InvalidInput_ReturnsError()
        {
            string timeStart = "2023-01-01T17:00:00";
            string timeEnd = "2023-01-01T15:00:00";

            string result = Helpers.CodingTime(timeStart, timeEnd);

            Assert.AreEqual("-02:00:00", result);
        }
        [TestMethod]
        public void GetUserInput_ValidInput_ReturnValidInput()
        {
            StringReader stringReader = new StringReader("30");
            Console.SetIn(stringReader);

            int result = UserInput.GetNumberInput("Enter a number");

            Assert.AreEqual(30,result);
        }
        [TestMethod]
        public void GetUserInput_InvalidInputThenValidInput_ReturnValidInput()
        {
            StringReader stringReader = new StringReader("invalid\n100");
            Console.SetIn(stringReader);

            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            int result = UserInput.GetNumberInput("Enter a number");

            Assert.AreEqual(100, result);
            string output = stringWriter.ToString();
            Assert.IsTrue(output.Contains("Enter a number"));
            Assert.IsTrue(output.Contains("Invalid number. Try again"));
        }
    }
}