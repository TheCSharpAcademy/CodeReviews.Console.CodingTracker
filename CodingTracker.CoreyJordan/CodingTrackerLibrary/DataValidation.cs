namespace CodingTrackerLibrary;
public static class DataValidation
{
    public static bool IsInRange(int key, List<CodingSessionModel> sessions)
    {
        if (sessions.Any(x => x.SessionId == key))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsNotFuture(DateTime date)
    {
        if (date > DateTime.Now)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static bool IsChronological(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
