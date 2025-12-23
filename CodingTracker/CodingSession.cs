using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace CodingTracker
{
    internal class CodingSession
    {
        internal int Id{set; get;}
        internal string StartTime { set; get; }
        internal string EndTime { set; get; }
        internal string Duration { set; get; }
        
        // Parameterless constructor for Dapper Query
        public CodingSession() { }
        internal CodingSession(string startTime, string endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            TimeSpan dur = HelperFunctions.CalculateDuration(StartTime, EndTime);
            Duration = dur.ToString("d'd 'h'h 'm'm 's's'");
        }
        public string DisplaySession()
        {
            string text = $"Coding Session #{Id}: Duration:{Duration}, Started: {StartTime} - Ended: {EndTime}\n";
            return text; 
        }
    }
}
