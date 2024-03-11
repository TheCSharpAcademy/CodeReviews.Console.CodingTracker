namespace CodingTracker.Exceptions;

public class CodingTrackerException : Exception
{
    public CodingTrackerException(string message) : base(message)
    {
    }
    
    public CodingTrackerException(string message, Exception innerException) : base(message, innerException)
    {
    }
}