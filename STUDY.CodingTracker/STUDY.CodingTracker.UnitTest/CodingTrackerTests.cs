using STUDY.CodingTracker.Helper;

namespace STUDY.CodingTracker.UnitTest;

public class CodingTrackerTests
{
    private static readonly object[] isValidDate =
    {
        new TestCaseData("05/05/2026 10:35", (true, Convert.ToDateTime("05/05/2026 10:35"))),

        new TestCaseData("05:05:2026 10:35", (false, new DateTime())),
        new TestCaseData("zefzf", (false, new DateTime())),
        new TestCaseData("zoief 10:35", (false, new DateTime())),
        new TestCaseData("05/05/2026 fzef", (false, new DateTime())),
        new TestCaseData("05/05/2026", (false, new DateTime())),
        new TestCaseData("10:35", (false, new DateTime())),
        new TestCaseData("32/05/2026 10:35", (false, new DateTime())),
        new TestCaseData("00/05/2026 10:35", (false, new DateTime())),
        new TestCaseData("05/13/2026 10:35", (false, new DateTime())),
        new TestCaseData("05/00/2026 10:35", (false, new DateTime())),
        new TestCaseData("05/05/2026 25:35", (false, new DateTime())),
        new TestCaseData("05/05/2026 -01:35", (false, new DateTime())),
        new TestCaseData("05/05/2026 10:66", (false, new DateTime())),
        new TestCaseData("05/05/2026 10:-01", (false, new DateTime()))
    };

    private static readonly object[] isValidEndDate =
    {
        new TestCaseData("05/05/2026 10:35", Convert.ToDateTime("04/05/2026 10:35"), (true, Convert.ToDateTime("05/05/2026 10:35"))),
        new TestCaseData("05/05/2026 10:35", Convert.ToDateTime("05/04/2026 10:35"), (true, Convert.ToDateTime("05/05/2026 10:35"))),
        new TestCaseData("05/05/2026 10:35", Convert.ToDateTime("05/05/2025 10:35"), (true, Convert.ToDateTime("05/05/2026 10:35"))),
        new TestCaseData("05/05/2026 10:35", Convert.ToDateTime("05/05/2026 09:35"), (true, Convert.ToDateTime("05/05/2026 10:35"))),
        new TestCaseData("05/05/2026 10:35", Convert.ToDateTime("05/05/2026 10:30"), (true, Convert.ToDateTime("05/05/2026 10:35"))),

        new TestCaseData("05/05/2026 10:35", Convert.ToDateTime("05/05/2026 10:35"), (false, Convert.ToDateTime("05/05/2026 10:35"))),
        new TestCaseData("05/05/2026 10:35", Convert.ToDateTime("06/05/2026 10:35"), (false, Convert.ToDateTime("05/05/2026 10:35"))),
        new TestCaseData("05/05/2026 10:35", Convert.ToDateTime("05/06/2026 10:35"), (false, Convert.ToDateTime("05/05/2026 10:35"))),
        new TestCaseData("05/05/2026 10:35", Convert.ToDateTime("05/05/2027 10:35"), (false, Convert.ToDateTime("05/05/2026 10:35"))),
        new TestCaseData("05/05/2026 10:35", Convert.ToDateTime("05/05/2026 11:35"), (false, Convert.ToDateTime("05/05/2026 10:35"))),
        new TestCaseData("05/05/2026 10:35", Convert.ToDateTime("05/05/2026 10:40"), (false, Convert.ToDateTime("05/05/2026 10:35"))),
    };

    private static readonly object[] isValidId =
    {
        new TestCaseData("5", (true, 5)),

        new TestCaseData("rerg", (false, 0)),
        new TestCaseData("-1", (false, -1)),
        new TestCaseData("0", (false, 0))
    };

    private static readonly object[] isValidPeriod =
    {
        new TestCaseData("5", (true, 5)),
        new TestCaseData("gerg", (false, 0))
    };

    private static readonly object[] isValidWeek =
    {
        new TestCaseData("3", (true, 3)),

        new TestCaseData("ziufhz", (false, 0)),
        new TestCaseData("-1", (false, -1)),
        new TestCaseData("0", (false, 0)),
        new TestCaseData("54", (false, 54))
    };

    private static readonly object[] isValidDay =
    {
        new TestCaseData("3", (true, 3)),

        new TestCaseData("ziufhz", (false, 0)),
        new TestCaseData("-1", (false, -1)),
        new TestCaseData("0", (false, 0)),
        new TestCaseData("32", (false, 32))
    };

    private static readonly object[] isValidYear =
    {
        new TestCaseData("2020", (true, 2020)),

        new TestCaseData("ziufhz", (false, 0)),
        new TestCaseData("1899", (false, 1899)),
        new TestCaseData("2050", (false, 2050)),
    };

    [TestCaseSource(nameof(isValidDate))]
    public void CorrectDate_ReturnsExpectedResult(string? input, (bool, DateTime) expectedValue)
    {
        (bool, DateTime) isValidDate = Verification.VerifyDate(input);
        Assert.That(isValidDate, Is.EqualTo(expectedValue));
    }

    [TestCaseSource(nameof(isValidEndDate))]
    public void CorrectEndDate_ReturnsExpectedResult(string? endDateInput, DateTime startDate, (bool, DateTime) expectedValue)
    {
        (bool, DateTime) isValidEndDate = Verification.VerifyEndDate(endDateInput, startDate);
        Assert.That(isValidEndDate, Is.EqualTo(expectedValue));
    }

    [TestCaseSource(nameof(isValidId))]
    public void CorrectId_ReturnsExpectedResult(string? input, (bool, int) expectedValue)
    {
        (bool, int) isValidId = Verification.VerifyId(input);
        Assert.That(isValidId, Is.EqualTo(expectedValue));
    }

    [TestCaseSource(nameof(isValidWeek))]
    public void CorrectWeek_ReturnsExpectedResult(string? input, (bool, int) expectedValue)
    {
        (bool, int) isValidWeek = Verification.VerifyWeek(input);
        Assert.That(isValidWeek, Is.EqualTo(expectedValue));
    }

    [TestCaseSource(nameof(isValidDay))]
    public void CorrectDay_ReturnsExpectedResult(string? input, (bool, int) expectedValue)
    {
        (bool, int) isValidDay = Verification.VerifyDay(input);
        Assert.That(isValidDay, Is.EqualTo(expectedValue));
    }

    [TestCaseSource(nameof(isValidYear))]
    public void CorrectYear_ReturnsExpectedResult(string? input, (bool, int) expectedValue)
    {
        (bool, int) isValidYear = Verification.VerifyYear(input);
        Assert.That(isValidYear, Is.EqualTo(expectedValue));
    }
}
