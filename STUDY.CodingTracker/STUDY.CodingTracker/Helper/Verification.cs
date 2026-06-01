using System.Globalization;

namespace STUDY.CodingTracker.Helper;

public static class Verification
{
    public static (bool correct, DateTime date) VerifyDate(string dateInput)
    {
        bool correct = false;
        DateTime date = new DateTime();

        if (dateInput != null && DateTime.TryParseExact(dateInput, "dd/MM/yyyy HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out date))
        {
            correct = true;
        }

        return (correct, date);
    }

    public static (bool correct, DateTime dateOnly) VerifyPeriodDate(string periodDate)
    {
        bool correct = false;
        DateTime date = new DateTime();

        if (periodDate != null && DateTime.TryParseExact(periodDate, "dd/MM/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out date))
        {
            correct = true;
        }

        return (correct, date);
    }

    public static (bool correct, DateTime date) VerifyEndDate(string endDateInput, DateTime startDate)
    {
        bool correct = false;
        var formatVerificationResult = VerifyDate(endDateInput);

        if (formatVerificationResult.correct && DateTime.Compare(formatVerificationResult.date, startDate) > 0)
        {
            correct = true;
        }

        return (correct, formatVerificationResult.date);
    }

    public static (bool correct, int id) VerifyId(string idInput)
    {
        bool correct = false;
        int id = 0;

        if (idInput != null && int.TryParse(idInput, out id) && id > 0)
        {
            correct = true;
        }

        return (correct, id);
    }

    private static (bool, int) VerifyPeriod(string periodInput)
    {
        bool correct = false;
        int periodNum = 0;

        if (periodInput != null && int.TryParse(periodInput, out periodNum))
        {
            correct = true;
        }
        return (correct, periodNum);
    }

    public static (bool correct, int periodNum) VerifyWeek(string weekInput)
    {
        bool correct = false;
        (bool correct, int weekNum) basicVerificationResult = VerifyPeriod(weekInput);

        if (basicVerificationResult.correct && basicVerificationResult.weekNum > 0 && basicVerificationResult.weekNum < 54)
            correct = true;

        return (correct, basicVerificationResult.weekNum);
    }

    public static (bool correct, int periodNum) VerifyDay(string dayInput)
    {
        bool correct = false;
        (bool correct, int dayNum) basicVerificationResult = VerifyPeriod(dayInput);

        if (basicVerificationResult.correct && basicVerificationResult.dayNum > 0 && basicVerificationResult.dayNum < 32)
            correct = true;

        return (correct, basicVerificationResult.dayNum);
    }

    public static (bool correct, int periodNum) VerifyYear(string yearInput)
    {
        bool correct = false;
        (bool correct, int yearNum) basicVerificationResult = VerifyPeriod(yearInput);

        if (basicVerificationResult.correct && basicVerificationResult.yearNum >= 1900 && basicVerificationResult.yearNum <= DateTime.Now.Year)
            correct = true;

        return (correct, basicVerificationResult.yearNum);
    }
}
