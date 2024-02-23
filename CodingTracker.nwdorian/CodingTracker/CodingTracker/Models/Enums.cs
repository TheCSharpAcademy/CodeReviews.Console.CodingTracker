namespace CodingTracker.Models;

internal class Enums
{
    public enum MenuSelection
    {
        LiveSession,
        ManageRecords,
        ViewAllRecords,
        ViewReports,
        SetGoal,
        ViewGoals,
        CloseApplication
    }

    public enum RecordsSelection
    {
        AddRecord,
        UpdateRecord,
        DeleteRecord,
        MainMenu
    }

    public enum UpdatingSelection
    {
        UpdateStartTime,
        UpdateEndTime,
        SaveChanges,
        MainMenu
    }

    public enum ReportSelection
    {
        Weekly,
        Monthly,
        Yearly,
        MainMenu
    }
}
