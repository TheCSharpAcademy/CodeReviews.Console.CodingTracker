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
}
