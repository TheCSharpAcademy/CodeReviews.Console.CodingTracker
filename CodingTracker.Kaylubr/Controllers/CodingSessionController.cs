using CodingTracker.Utils;

namespace CodingTracker.Controllers;

internal static class CodingTrackerController
{
    internal static void InsertSession()
    {
        var (startTime, endTime) = UserInput.GetStartAndEndTime();
        string duration = UserInput.GetDuration(startTime, endTime);
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
        bool exists = Helper.RenderCodingSessionInTable(Database.GetAll());

        if (exists)
        {
            int id = UserInput.GetID("EDITED");

            if (!Database.FindOneSession(id))
            {
                Helper.Pause("Record not found!", success: false);
                return;
            }

            var (startTime, endTime) = UserInput.GetStartAndEndTime();
            string duration = UserInput.GetDuration(startTime, endTime);

            Database.Update(id, startTime, endTime, duration);

            Helper.Pause("Successful Operation!", success: true);
        }
        else
        {
            Helper.Pause();
        }
    }

    internal static void DeleteRecord()
    {
        bool exists = Helper.RenderCodingSessionInTable(Database.GetAll());

        if (exists)
        {
            int id = UserInput.GetID("DELETED");

            if (!Database.FindOneSession(id))
            {
                Helper.Pause("Record not found!", success: false);
                return;
            }

            if (!Helper.Confirmation("Are you sure?"))
            {
                return;
            }

            Database.DeleteOne(id);

            Helper.Pause("Successful Operation!", success: true);
        }
        else
        {
            Helper.Pause();
        }
    }
}