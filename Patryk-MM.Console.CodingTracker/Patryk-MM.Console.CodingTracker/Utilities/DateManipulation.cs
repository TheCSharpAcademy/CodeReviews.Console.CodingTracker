using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patryk_MM.Console.CodingTracker.Utilities {
    public static class DateManipulation {
        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan) {
            if (timeSpan == TimeSpan.Zero)
                return dateTime;

            if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue)
                return dateTime;

            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }
    }
}
