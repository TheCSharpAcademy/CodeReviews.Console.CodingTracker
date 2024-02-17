using System.ComponentModel.DataAnnotations;

namespace Doc415.CodingTracker;

internal class Enums
{
    public enum MainMenuSelections
    {
        [Display(Name = "Start live coding session")]
        LiveCodingSession,
        [Display(Name = "Set goals")]
        SetGoal,
        [Display(Name = "Add new record")]
        AddRecord,
        [Display(Name = "View records")]
        ViewRecords,
        [Display(Name = "Delete record")]
        DeleteRecord,
        [Display(Name = "Update record")]
        UpdateRecord,
        [Display(Name = "Statistics")]
        Statistics,
        [Display(Name = "Report Goal")]
        ReportGoals,
        Quit
    }
}
