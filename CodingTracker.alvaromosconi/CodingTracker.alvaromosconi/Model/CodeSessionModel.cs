using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.alvaromosconi.Model;

internal class CodeSessionModel
{
    internal int Id { get; set; }
    internal DateTime StartDateTime { get; set; }
    internal DateTime EndDateTime { get; set; }
    internal TimeSpan Duration
    {
        get
        {
            return CalculateDuration();
        }
    }

    private TimeSpan CalculateDuration()
    {
        return EndDateTime - StartDateTime;
    }
}
