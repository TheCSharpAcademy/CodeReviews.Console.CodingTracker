
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

        public static void MonitorHomeKey()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Home)
                    {
                        UserInterface.MainMenu();
                    }
                }
            }
        }
    }
}
