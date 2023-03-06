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
            if ((c.StartDateTime.Date >= startDate && c.StartDateTime.Date <= endDate) ||
                (c.EndDateTime.Date <= endDate && c.EndDateTime.Date >= startDate))
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

    public TimeSpan TotalTimeBetweenDates(List<CodingSession> listToOperate, DateTime startDate, DateTime endDate)
    {
        if (listToOperate is null || startDate == DateTime.MinValue || endDate == DateTime.MinValue) return TimeSpan.Zero;

        long totalTicks = 0;
        foreach (CodingSession c in listToOperate)
        {
            long auxStartTicks = 0, auxEndTicks = 0;
            if (c.EndDateTime.Ticks <= startDate.Ticks || c.StartDateTime.Ticks >= endDate.Ticks) continue;
            if(c.StartDateTime.Ticks <= startDate.Ticks) { auxStartTicks = startDate.Ticks; }
            else { auxStartTicks = c.StartDateTime.Ticks; }

            if(c.EndDateTime.Ticks <= endDate.Ticks) { auxEndTicks = c.EndDateTime.Ticks; }
            else { auxEndTicks = c.EndDateTime.Ticks; }

            totalTicks += auxEndTicks - auxStartTicks;
        }

        TimeSpan returnTime = new TimeSpan(totalTicks);
        return returnTime;
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
