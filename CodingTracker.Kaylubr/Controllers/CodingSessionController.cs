using CodingTracker.Models;
using CodingTracker.Utils;

namespace CodingTracker.Controllers;

internal static class CodingTrackerController
{
    internal static void InsertSession()
    {
        DateTime startTime;
        DateTime endTime;

        do
        {
            startTime = Helper.GetTime("start");
            endTime = Helper.GetTime("end");
        } while (!Helper.ValidateTime(startTime, endTime));

        string duration = Helper.GetDuration(startTime, endTime);

        string startTimeInString = startTime.ToString();
        string endTimeInString = endTime.ToString();

        Database.Insert(startTimeInString, endTimeInString, duration);

        Helper.PrintSuccessOperation();
    }

    internal static void LogAllRecords()
    {
        List<CodingSession> codingSessions = Database.GetAll();
        Helper.RenderCodingSessionInTable(codingSessions);
    }
}