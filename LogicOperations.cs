using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTracker
{
    public static class LogicOperations
    {
    public static string ConstructDateTime(string timeInput, string dateInput) => DateTime.Parse(dateInput + " " + timeInput).ToString("MM/DD/yyyy HH:mm");        

    }
}
