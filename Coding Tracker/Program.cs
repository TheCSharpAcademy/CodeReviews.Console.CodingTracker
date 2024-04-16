using System;

namespace CodingTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Database.CreateDatabase();
            CodingTrackerController.Menu();
        }
    }
   
}