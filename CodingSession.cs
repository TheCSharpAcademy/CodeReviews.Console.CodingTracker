using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTracker
{
    public class CodingSession
    {

        public int Id { get; set; }
        public DateTime StartTime {get; set;}
        public DateTime EndTime {get; set;}
        public TimeSpan Duration {get; set;}
        public string Note {get; set;}
    }
}
