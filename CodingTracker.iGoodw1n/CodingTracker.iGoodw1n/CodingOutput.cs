using ConsoleTableExt;
using System.Globalization;

namespace CodingTracker.iGoodw1n;

public static class CodingOutput
{
    public static void ShowOnConsole(InfoForOutput infoForOutput)
    {
        switch (infoForOutput.Type)
        {
            case InfoType.Text:
                Console.WriteLine();
                Console.WriteLine(infoForOutput.Information);
                Console.WriteLine();
                break;
            case InfoType.AllSessions:
                ShowAllSessions((List<CodingSession>?)infoForOutput.Information);
                break;
            case InfoType.AnnualReport:
                ShowAnnualReport((List<CodingSession>?)infoForOutput.Information);
                break;
            case InfoType.Years:
                ShowYears((List<int>?)infoForOutput.Information);
                break;
            case InfoType.OneSession:
                ShowSession((CodingSession?)infoForOutput.Information);
                break;
            default:
                throw new ArgumentException(nameof(infoForOutput));
        }
    }

    private static void ShowSession(CodingSession? session)
    {
        if (session is null)
        {
            Console.WriteLine("There is no data for this ID in DB");
        }
        else
        {
            ShowAllSessions(new List<CodingSession> { session });
        }
    }

    private static void ShowYears(List<int>? information)
    {
        if (information is null)
        {
            Console.WriteLine("There is no records in DB");
        }
        else
        {
            Console.WriteLine($"Enter a year from this list ({string.Join(", ", information)}) to get report for:");
        }
    }

    private static void ShowAllSessions(List<CodingSession>? sessions)
    {
        if (sessions is null)
        {
            Console.WriteLine("There is no data in DB");
            return;
        }

        var tableData = sessions.Select(s => new List<object> { s.Id, s.Language, s.Start, s.End, s.Duration }).ToList();
        tableData.Insert(0, new List<object> { "Id", "Programming Language", "Start Time", "End Time", "Duration" });
        ConsoleTableBuilder
            .From(tableData)
            .ExportAndWriteLine();
    }

    private static void ShowAnnualReport(List<CodingSession>? sessionsForYear)
    {
        if (sessionsForYear is null || !sessionsForYear.Any())
        {
            Console.WriteLine("There is no data for this year");
            return;
        }

        var tableData = new List<List<object>>
        {
            Enumerable.Range(1, 12)
                .Select(n => (object)CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(n))
                .ToList()
        };

        foreach (var allSessionsForLanguage in sessionsForYear.GroupBy(s => s.Language))
        {
            var langData = new List<object>();
            var lang = allSessionsForLanguage.First().Language;
            foreach (var month in Enumerable.Range(1, 12))
            {
                langData.Add(string.Format("{0}: {1:0}",
                    lang,
                    allSessionsForLanguage
                        .Where(s => s.Start.Month == month)
                        .Aggregate(TimeSpan.Zero, (r, s) => r + (s.End - s.Start)).TotalMinutes
                ));
            }

            tableData.Add(langData);
        }

        ConsoleTableBuilder
            .From(tableData)
            .WithTitle($"Annual report for {sessionsForYear[0].Start.Year}. (Values in minutes)")
            .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
            .ExportAndWriteLine();
    }
}
