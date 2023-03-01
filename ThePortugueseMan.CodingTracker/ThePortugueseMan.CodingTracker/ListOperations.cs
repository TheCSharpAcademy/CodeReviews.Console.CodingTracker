using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePortugueseMan.CodingTracker;

internal class ListOperations
{
    public List<CodingSession> ReturnOrderedByAscendingDate(List<CodingSession> listToOrder)
    {
        List<CodingSession> returnList = listToOrder.OrderBy(c => c.StartDateTime).ToList();
        return returnList;
    }
    public List<CodingSession> ReturnOrderedByDescendingDate(List<CodingSession> listToOrder)
    {
        List<CodingSession> returnList = listToOrder.OrderByDescending(c => c.StartDateTime).ToList();
        return returnList;
    }

    public List<CodingSession> ReturnLogsBetweenDates(List<CodingSession> listToOperate, DateTime startDate, DateTime endDate)
    {
        List<CodingSession> returnList = new List<CodingSession>();

        foreach (CodingSession c in listToOperate)
        {
            if (c.StartDateTime.Year >= startDate.Year && c.EndDateTime.Year <= endDate.Year)
            {
                if (c.StartDateTime.Month >= startDate.Month && c.EndDateTime.Month <= endDate.Month)
                {
                    if (c.StartDateTime.Day >= startDate.Day && c.EndDateTime.Day <= endDate.Day)
                    {
                        returnList.Add(c);
                    }
                }
            }
            else continue;
        }
        return returnList;
    }

    public TimeSpan TotalTimeInList(List<CodingSession> listToOperate)
    {
        TimeSpan totalTime = TimeSpan.Zero;
        foreach (CodingSession c in listToOperate)
        {
            totalTime.Add(c.Duration);
        }
        return totalTime;
    }

    public TimeSpan AverageTimeInList(List<CodingSession> listToOperate)
    {
        TimeSpan totalTime = TotalTimeInList(listToOperate);
        TimeSpan averageTime = totalTime / listToOperate.Count();
        return averageTime;
    }
}
