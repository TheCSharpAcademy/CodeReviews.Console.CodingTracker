namespace CodingTracker
{
    internal class Verifier
    {
        public static void VerifyDate(DateTime startTime, DateTime endTime)
        {
            CoddingSession session = new(-1, startTime, endTime);
            if(session.CalculateDuration() < 0)
            {
                throw new ArgumentException("Duration must be positive, please enter correct start or end date");
            }
        }
    }
}
