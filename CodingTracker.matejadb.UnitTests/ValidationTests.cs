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
    }
}
