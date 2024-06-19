using System;
using Spectre.Console;
namespace CodingTracker
{
    public class CodingSession
    {
        public int Id { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public int duration { get; set; }
        public int CalculateDuration(string start, string end)
        {
            Validation validation = new Validation();
            int a = validation.ValidString(start);
            int b = validation.ValidString(end);
            return b-a;
        }

    }
}
