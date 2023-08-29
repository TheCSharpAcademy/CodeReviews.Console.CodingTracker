using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.w0lvesvvv
{
    public static class UserInput
    {
        public static int? readNumber()
        {
            string inputNumber = Console.ReadLine() ?? string.Empty;
            if (!Validation.validateNumber(inputNumber, out int parsedNumber)) return null;

            return parsedNumber;
        }

        public static string readDateTimeString() { 
            string inputDateTime = Console.ReadLine() ?? string.Empty;

            if (!Validation.validateDateTimeString(inputDateTime)) return string.Empty;

            return inputDateTime;
        }
    }
}
