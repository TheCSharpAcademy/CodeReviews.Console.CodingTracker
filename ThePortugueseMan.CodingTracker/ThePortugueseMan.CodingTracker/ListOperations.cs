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
    public List<CodingSession> ReturnLogsBetweenDates(List<CodingSession> listToOrder, DateTime startDate, DateTime endDate)
    {
        return new List<CodingSession>();
    }
}
