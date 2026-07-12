using CodingTracker.matejadb.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.matejadb.UnitTests; 
public class DurationCalculationTests {
    [Test]
    public void ValidDateTimeInputs_ReturnsCorrectlyCalculatedDurationInMinutes() {
        var startTime = "2026-07-12 13:30";
        var endTime = "2026-07-12 14:00";

        var duration = CalculateSessionDuration.SessionDuration(startTime, endTime);

        Assert.That(duration, Is.EqualTo("30 minutes"));
    }
}
