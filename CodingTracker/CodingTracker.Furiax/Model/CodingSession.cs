using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.Furiax.Model
{
	internal class CodingSession
	{
        public int Id { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public TimeSpan Duration => EndTime - StartTime;
    }
}
