using CodeTracker;
using System.Globalization;

namespace UnitTest.Speedierone
{
    [TestClass]
    public class CodingTests
    {
        [TestMethod]
        public void IsValidDate_ReturnsFalse()
        {
            var invalidDates = new List<string>
            {
                "14/14/2022",
                "14-14-22",
                "14-12-2022",
                "14/12/2022",
                "Invalid date",
                "2022/12/12",
                "",
                " ",
                "1",
                "a"
            };

            foreach (var invalidDate in invalidDates)
            {
                bool result = Helpers.IsValidDate(invalidDate);

                Assert.IsFalse(result);
            }
        }
        [TestMethod]
        public void IsValidDate_ReturnsTrue()
        {
            var validDates = new List<string>
            {
                "12-12-22",
                "01-10-20",
                "",
                " ",
                "1",
                "a"
            };

            foreach(var validDate in validDates)
            {
                bool result = Helpers.IsValidDate(validDate);
                Assert.IsTrue(result);
            }
        }
        [TestMethod]
        public void NotValidTime_ReturnFalse()
        {
            var invalidTimes = new List<string>
            {
                "50-10",
                "12:00",
                "12/00",
                "12-100",
                "Invalid Time",
                "15:00",
                "",
                " ",
                "1",
                "a"
            };

            foreach (var invalidTime in invalidTimes)
            {
                bool result = Helpers.IsValidTime(invalidTime);
                Assert.IsFalse(result);
            }
        }
        [TestMethod]
        public void IsValidTime_ReturnTrue()
        {
            var validTimes = new List<string>
            {
                "12-00",
                "11-59",
                "21-00",
                "",
                " ",
                "1",
                "a"
            };

            foreach (var validTime in validTimes)
            {
                bool result = Helpers.IsValidTime(validTime);
                Assert.IsTrue(result);
            }
        }
        [TestMethod]
        public void EndTimeBeforeStartTime_ReturnsFalse()
        {
            DateTime timeStart1;
            DateTime timeEnd1;
            DateTime timeStart2;
            DateTime timeEnd2;

            bool parsedTimeStart1 = DateTime.TryParseExact("21-01-2023T12-00", "dd-MM-yy HH-mm", new CultureInfo("en-GB"), DateTimeStyles.None, out timeStart1);
            bool parsedTimeEnd1 = DateTime.TryParseExact("21-01-2023T11-00", "dd-MM-yy HH-mm", new CultureInfo("en-GB"), DateTimeStyles.None, out timeEnd1);
            bool parsedTimeStart2 = DateTime.TryParseExact("21-01-2023T15:00", "dd-MM-yy HH-mm", new CultureInfo("en-GB"), DateTimeStyles.None, out timeStart2);
            bool parsedTimeEnd2 = DateTime.TryParseExact("21-01-2022T16:00", "dd-MM-yy HH-mm", new CultureInfo("en-GB"), DateTimeStyles.None, out timeEnd2);

            if(parsedTimeStart1 && parsedTimeEnd1)
            {
                bool result = UserInput.CheckDate(timeStart1, timeEnd1);
                Assert.IsFalse(result);
            }
            if(parsedTimeStart2 && parsedTimeEnd2)
            {
                bool result = UserInput.CheckDate(timeStart2, timeEnd2);
                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public void EndTimeAfterStartTime_ReturnTrue()
        {
            DateTime timeStart1;
            DateTime timeEnd1;
            DateTime timeStart2;
            DateTime timeEnd2;

            bool parsedTimeStart1 = DateTime.TryParseExact("21-01-2023T12-00", "dd-MM-yy HH-mm", new CultureInfo("en-GB"), DateTimeStyles.None, out timeStart1);
            bool parsedTimeEnd1 = DateTime.TryParseExact("21-01-2023T15-00", "dd-MM-yy HH-mm", new CultureInfo("en-GB"), DateTimeStyles.None, out timeEnd1);
            bool parsedTimeStart2 = DateTime.TryParseExact("21-01-2023T15:00", "dd-MM-yy HH-mm", new CultureInfo("en-GB"), DateTimeStyles.None, out timeStart2);
            bool parsedTimeEnd2 = DateTime.TryParseExact("21-01-2023T16:00", "dd-MM-yy HH-mm", new CultureInfo("en-GB"), DateTimeStyles.None, out timeEnd2);

            if (parsedTimeStart1 && parsedTimeEnd1)
            {
                bool result = UserInput.CheckDate(timeStart1, timeEnd1);
                Assert.IsTrue(result);
            }
            if (parsedTimeStart2 && parsedTimeEnd2)
            {
                bool result = UserInput.CheckDate(timeStart2, timeEnd2);
                Assert.IsTrue(result);
            }
        }
    }
}