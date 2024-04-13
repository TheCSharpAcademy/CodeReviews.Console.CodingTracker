namespace CodingTracker.Database.Helpers;

public class DateTimeHelper
{
  public static int CalculateDuration(string startDate, string endDate)
  {
    return Convert.ToInt32((Convert.ToDateTime(endDate) - Convert.ToDateTime(startDate)).TotalMinutes);
  }
}