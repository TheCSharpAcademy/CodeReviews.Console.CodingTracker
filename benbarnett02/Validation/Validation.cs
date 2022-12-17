namespace TrackingProgram;
public class Validation
{
    public static bool DateTimeIsValid(DateTime startTime, DateTime endTime)
    {
        if (startTime < endTime) { return true; } else { return false; }
    }
}