using CodingTracker.Utils;

namespace CodingTracker.Controllers;

internal static class CodingTrackerController
{
    internal static void InsertSession()
    {
        var (startTime, endTime) = Helper.GetStartAndEndTime();
        string duration = Helper.GetDuration(startTime, endTime);
        Database.Insert(startTime, endTime, duration);
        Helper.Pause("Successful Operation!", success: true);
    }

    internal static void LogAllRecords()
    {
        Helper.RenderCodingSessionInTable(Database.GetAll());
        Helper.Pause();
    }

    internal static void UpdateRecord()
    {
        Helper.RenderCodingSessionInTable(Database.GetAll());

        int id = Helper.GetID("EDITED");

        if (!Database.FindOneSession(id))
        {
            Helper.Pause("Record not found!", success: false);
            return;
        }

        var (startTime, endTime) = Helper.GetStartAndEndTime();
        string duration = Helper.GetDuration(startTime, endTime);

        Database.Update(id, startTime, endTime, duration);

        Helper.Pause("Successful Operation!", success: true);
    }

    internal static void DeleteRecord()
    {
        Helper.RenderCodingSessionInTable(Database.GetAll());

        int id = Helper.GetID("DELETED");

        if (!Helper.Confirmation("Are you sure?"))
        {
            return;
        }

        Database.DeleteOne(id);

        if (!Database.FindOneSession(id))
        {
            Helper.Pause("Record not found!", success: false);
            return;
        }

        Helper.Pause("Successful Operation!", success: true);
    }
}