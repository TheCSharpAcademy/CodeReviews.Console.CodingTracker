using CodingTracker.matejadb.Utils;

namespace CodingTracker.matejadb.UnitTests {
    public class ValidationTests {

        [Test]
        public void CorrectDateTimeInput_ReturnsTrue() {

            var isValidDateTime = Validation.ValidateDateTime("2026-07-12 13:30");

            Assert.That(isValidDateTime, Is.True);
        }

        [Test]
        public void InvalidDateTimeInput_ReturnsFalse() {
            var isValidDateTime = Validation.ValidateDateTime("2026-7-12 1:30");

            Assert.That(isValidDateTime, Is.False);

        }

        [Test]
        public void InputtingEndDateInThePast_ReturnsFalse() {
            var startDate = "2026-07-18 13:30";
            var endDate = "2026-07-17 13:30";

            var isValidEndDate = Validation.ValidateEndDate(startDate, endDate);

            Assert.That(isValidEndDate, Is.False);
        }

        [Test]
        public void InputtingCorrectEndDate_ReturnsTrue() {
            var startDate = "2026-07-18 13:30";
            var endDate = "2026-07-18 15:00";

            var isValidEndDate = Validation.ValidateEndDate(startDate, endDate);

            Assert.That(isValidEndDate, Is.True);
        }
    }
}
