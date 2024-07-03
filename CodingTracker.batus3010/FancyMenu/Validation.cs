using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class Validation
    {
        public static bool IsValidDateTime(string input, string expectedFormat)
        {
            return DateTime.TryParseExact(input, expectedFormat, null, System.Globalization.DateTimeStyles.None, out _);
        }
    }
}
