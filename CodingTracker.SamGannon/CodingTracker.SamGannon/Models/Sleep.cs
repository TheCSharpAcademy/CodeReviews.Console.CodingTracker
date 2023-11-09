using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.SamGannon.Models
{
    internal class Sleep
    {
        public int Id { get; set; }
        public string Duration { get; set; }
        public SleepType sleepType { get; set; }
    }

    public enum SleepType
    {
        LongRest,
        ShortRest
    }
}
