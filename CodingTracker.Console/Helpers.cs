namespace CodingTracker;

public static class Helpers
{
    public static int CalculateSecondsBetweenDates(string startDate, string endDate)
    {
        var dtOne = DateTime.ParseExact(startDate, "HH:mm dd-MM-yy", null, System.Globalization.DateTimeStyles.None);
        var dtTwo = DateTime.ParseExact(endDate, "HH:mm dd-MM-yy", null, System.Globalization.DateTimeStyles.None);

        TimeSpan duration = dtTwo - dtOne;
        double durationSeconds = duration.TotalSeconds;

        return (int)durationSeconds;
    }
}
