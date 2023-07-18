using System;
using System.Linq;

namespace CodingTracker.Wolffles;

public interface ISession
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan Duration { get; set; }

    public void CalculateDuration();
}
