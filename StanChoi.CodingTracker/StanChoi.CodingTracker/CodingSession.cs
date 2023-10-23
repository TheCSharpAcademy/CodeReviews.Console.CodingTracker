using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StanChoi.CodingTracker
{
    internal class CodingSession
    {
        public int Id { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Duration { get; set; }
    }
}
