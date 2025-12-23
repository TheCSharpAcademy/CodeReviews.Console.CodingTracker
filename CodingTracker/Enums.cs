using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    internal class Enums
    {
        internal enum MenuChoice
        {
            StartTime,
            InsertTime,
            CheckHistory,
            DeleteSession,
            DeleteHistory,
            Exit
        }
        internal enum SessionViewChoice
        {
            Normal,
            Chronologically,
            ReversedChronologically,
            SortedByDurationAscending,
            SortedByDurationDescending,
            Custom
        }
    }
}
