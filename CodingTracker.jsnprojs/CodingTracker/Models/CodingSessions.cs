using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.Models;
internal class CodingSessions
{
    internal int ID { get; set; }
    internal String StartTime { get; set; }
    internal String EndTime { get; set; }
    internal double Duration { get; set; }
}
