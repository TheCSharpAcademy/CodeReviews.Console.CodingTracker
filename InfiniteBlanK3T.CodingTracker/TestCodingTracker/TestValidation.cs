using CodingTracker;

namespace TestCodingTracker
{
    public class TestValidation
    {
        Validation testVal;
        List<int> testDuration;
        [SetUp]
        public void Setup()
        {
            testVal = new Validation();
            testDuration = new List<int>();
        }

        [Test]
        public void TestCheckDateInput()
        {
            Assert.IsTrue(testVal.CheckDateInput("11-01-01"));
            Assert.IsFalse(testVal.CheckDateInput("11-1-01"));
            Assert.IsFalse(testVal.CheckDateInput("32-12-01"));
            Assert.IsFalse(testVal.CheckDateInput("32-13-01"));
        }
        [Test]
        public void TestCheckHourInput()
        {
            Assert.IsTrue(testVal.CheckHourInput("12"));
            Assert.IsFalse(testVal.CheckHourInput("31"));
            Assert.IsFalse(testVal.CheckHourInput("-1"));
            Assert.IsFalse(testVal.CheckHourInput("24"));
        }
        [Test]
        public void TestCheckMinInput()
        {
            Assert.IsTrue(testVal.CheckMinInput("31"));
            Assert.IsFalse(testVal.CheckMinInput("-1"));
            Assert.IsFalse(testVal.CheckMinInput("60"));
        }
        [Test]
        public void TestCalculateDuration()
        {
            // From 8:30 AM - 5:30 PM
            testDuration.Add(8);
            testDuration.Add(30);
            testDuration.Add(17);
            testDuration.Add(30);
            Assert.AreEqual(540, testVal.CalculateDuration(testDuration));
            testDuration.Clear();
            //From 11:30 - 18:20
            testDuration.Add(11);
            testDuration.Add(30);
            testDuration.Add(18);
            testDuration.Add(20);
            Assert.AreEqual(410, testVal.CalculateDuration(testDuration));
            testDuration.Clear();
            //From 01:59 AM - 2:05 PM
            testDuration.Add(1);
            testDuration.Add(59);
            testDuration.Add(14);
            testDuration.Add(5);
            Assert.AreEqual(726, testVal.CalculateDuration(testDuration));
        }
    }
}