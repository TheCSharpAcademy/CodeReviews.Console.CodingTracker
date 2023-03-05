using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePortugueseMan.CodingTracker;

public class Goal
{
    public int Id;
    public DateTime StartDate;
    public DateTime EndDate;
    public TimeSpan TargetHours;
    public TimeSpan HoursSpent;
    public string Status;

}
