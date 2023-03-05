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

    public List<CodingSession> GetLogsBetweenDates(List<CodingSession> listToOperate, DateTime startDate, DateTime endDate)
    {
        if (listToOperate is null) return null;

        List<CodingSession> returnList = new List<CodingSession>();

        foreach (CodingSession c in listToOperate)
        {
            if (c.StartDateTime.Date >= startDate && c.EndDateTime.Date <= endDate)
            {
                returnList.Add(c);
            }
        }
        if (returnList.Count() == 0) return null;
        return returnList;
    }

    public TimeSpan TotalTimeInList(List<CodingSession> listToOperate)
    {
        if (listToOperate is null) return TimeSpan.Zero;

        TimeSpan totalTime = TimeSpan.Zero;

        foreach (CodingSession c in listToOperate)
        {
            totalTime = totalTime.Add(c.Duration);
        }
        return totalTime;
    }

    public TimeSpan AverageTimeInList(List<CodingSession> listToOperate)
    {
        TimeSpan totalTime = TotalTimeInList(listToOperate);
        TimeSpan averageTime = totalTime / listToOperate.Count();
        return averageTime;
    }

    public DateTime FirstDateInList(List<CodingSession> listToOperate)
    {
        List<CodingSession> auxList = ReturnOrderedByAscendingDate(listToOperate);
        return auxList.ElementAt(0).StartDateTime;
    }

    public DateTime LastDateInList(List<CodingSession> listToOperate)
    {
        List<CodingSession> auxList = ReturnOrderedByDescendingDate(listToOperate);
        return auxList.ElementAt(0).EndDateTime;
    }

    public TimeSpan DifferenceBetweenFirsAndLastDates(List<CodingSession> listToOperate)
    {
        TimeSpan returnTime = LastDateInList(listToOperate).Subtract(FirstDateInList(listToOperate));
        return returnTime;
    }

    public int NumberOfSessionsInList(List<CodingSession> listToOperate) { return listToOperate.Count(); }
}
