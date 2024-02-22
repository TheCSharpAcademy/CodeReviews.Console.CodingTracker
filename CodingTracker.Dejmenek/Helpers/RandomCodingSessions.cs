using CodingTracker.Dejmenek.Models;

namespace CodingTracker.Dejmenek.Helpers
{
    public static class RandomCodingSessions
    {
        private readonly static Random _random = new Random();

        public static List<CodingSession> GenerateRandomCodingSessions()
        {
            List<CodingSession> sessions = new List<CodingSession>();
            for (int i = 0; i < 10; i++)
            {
                (DateTime startDateTime, DateTime endDateTime) = RandomDate.GetRandomDateTimes();
                TimeSpan timeSpan = endDateTime - startDateTime;
                int duration = (int)timeSpan.TotalMinutes;
                sessions.Add(new CodingSession
                {
                    StartDateTime = startDateTime.ToString("yyyy-MM-dd HH:mm"),
                    EndDateTime = endDateTime.ToString("yyyy-MM-dd HH:mm"),
                    Duration = duration,
                });
            }

            return sessions;
        }
    }
}
