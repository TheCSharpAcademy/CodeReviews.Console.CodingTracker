using CodingTracker.Controllers;
using CodingTracker.Models;

namespace CodingTracker.Views;

using Spectre.Console;

public class TableVisualisationEngine
{
    public static void GenerateFullReport()
    {
        List<CodingSession> tableData = CrudManager.GetAllSessions();
        foreach (CodingSession row in tableData)
        {
            Console.WriteLine(
                $"{row.Id} - {row.StartTime} - {row.EndTime} - {row.Duration}");
        }
    }
}