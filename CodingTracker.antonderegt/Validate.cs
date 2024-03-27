namespace CodingTracker;

public class Validate
{
    public static bool CanEndSession(CodingSession session)
    {
        return !session.StartTime.Equals(DateTime.MinValue);
    }

    public static bool EndTimeIsAfterStartTime(CodingSession session, DateTime endTime)
    {
        return session.StartTime.CompareTo(endTime) < 0;
    }
}