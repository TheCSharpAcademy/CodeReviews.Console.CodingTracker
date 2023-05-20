using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    // Model/Entity for goal feature
    public class Goal
    {
        public int Id { get; set; }

        public TimeSpan GoalValue { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
