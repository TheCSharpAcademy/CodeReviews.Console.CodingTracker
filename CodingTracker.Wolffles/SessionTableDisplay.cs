using ConsoleTableExt;
using System.Collections.Generic;
using System.Linq;

namespace CodingTracker.Wolffles;

public class SessionTableDisplay
{
    private List<List<object>> CreateTable(List<ISession> list)
    {
        List<List<object>> sessionList = new List<List<object>>();

        //Need to convert ISession to a string List to be processed by ConsoleTableExt
        foreach (ISession session in list)
        {
            sessionList.Add(new List<object> { session.Id, session.StartDate.ToString(), session.EndDate.ToString(), session.Duration.ToString() });
        }

        return sessionList;
    }

    public void DisplayTable(List<ISession> list)
    {

        List<List<object>> sessionList = CreateTable(list);

        //Formatting for ConsoleTableExt
        ConsoleTableBuilder.From(sessionList)
        .WithTitle("Coding Sessions", ConsoleColor.Yellow)
        .WithColumn("Id", "Start Date", "End Date", "Duration")
        .WithTextAlignment(new Dictionary<int, TextAligntment>
        {
                {0, TextAligntment.Center },
                {1, TextAligntment.Center },
                {2, TextAligntment.Center },
                {3, TextAligntment.Center }
        })
        .WithMinLength(new Dictionary<int, int> {
                { 1, 25 },
                { 2, 25 },
                { 3, 25 }
        })
        .ExportAndWriteLine();

    }
}
