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

        TimeSpan duration = Helper.GetDuration(startTime, endTime);

        string startTimeInString = startTime.ToString();
        string endTimeInString = endTime.ToString();
        string durationInString = duration.ToString();

        Database.Insert(startTimeInString, endTimeInString, durationInString);

        Helper.PrintSuccessOperation();
    }
}