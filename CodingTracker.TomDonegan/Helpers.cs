
using System.Security.Cryptography;

namespace CodingTracker.TomDonegan
{
    internal class Helpers
    { 
        public static String CalculateDuration(DateTime startTime, DateTime endTime)
        {
            if (endTime < startTime)
            {
                endTime = endTime.AddDays(1);
            }

            TimeSpan duration = endTime - startTime;  

            return duration.ToString();
        }
    }
}
